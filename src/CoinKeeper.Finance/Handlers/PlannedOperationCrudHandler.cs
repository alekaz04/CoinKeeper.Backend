using AutoMapper;
using CoinKeeper.Authentication.Domain;
using CoinKeeper.Common;
using CoinKeeper.Infrastructure;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoinKeeper.Finance;

/// <summary>
/// Круд хэндлер для плановых операций
/// </summary>
public class PlannedOperationCrudHandler : AbstractCrudHandler<PlannedOperation, PlannedOperationReadDto, PlannedOperationCreateDto, PlannedOperationUpdateDto>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;
    private readonly IAccountBalanceService _balanceService;
    private readonly ILogger<PlannedOperationCrudHandler> _logger;

    public PlannedOperationCrudHandler(
        DataContext context,
        IMapper mapper,
        IValidator<PlannedOperationCreateDto> validator,
        ICurrentUser currentUser,
        IAccountBalanceService balanceService,
        ILogger<PlannedOperationCrudHandler> logger) : base(context, mapper, validator, currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
        _balanceService = balanceService;
        _logger = logger;
    }

    /// <summary>
    /// Ручное выполнение планового платежа
    /// </summary>
    public async Task<OperationReadDto> ExecutePlannedOperationAsync(Guid id, PlannedOperationExecuteDto executeDto, CancellationToken cancellationToken = default)
    {
        var currentUserId = _currentUser.GetCurrentUserId();
        var plannedOperation = await _context.Set<PlannedOperation>()
            .Include(x => x.Account)
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == currentUserId && !x.IsDeleted, cancellationToken);

        if (plannedOperation == null)
        {
            throw new CommonErrorException("Плановый платеж не найден");
        }

        if (!plannedOperation.IsActive)
        {
            throw new CommonErrorException("Плановый платеж неактивен");
        }

        if (plannedOperation.IsPaused)
        {
            throw new CommonErrorException("Плановый платеж приостановлен");
        }

        // Проверка лимита выполнений
        if (plannedOperation.MaxExecutions.HasValue &&
            plannedOperation.ExecutedCount >= plannedOperation.MaxExecutions.Value)
        {
            throw new CommonErrorException("Достигнут лимит выполнений планового платежа");
        }

        try
        {
            // Создание операции
            var operation = _mapper.Map<Operation>(plannedOperation);

            // Переопределение параметров если указаны
            if (executeDto.ExecutionDate.HasValue)
            {
                operation.OperationTime = executeDto.ExecutionDate.Value;
            }

            if (executeDto.OverrideAmount.HasValue)
            {
                operation.Amount = executeDto.OverrideAmount.Value;
            }

            if (!string.IsNullOrEmpty(executeDto.AdditionalDescription))
            {
                operation.Description = $"Ручное выполнение: {plannedOperation.Name}. {executeDto.AdditionalDescription}";
            }

            // Применение операции к балансу
            await _balanceService.ApplyOperationToBalance(currentUserId, operation, cancellationToken);

            // Обновление счетчика выполнений
            plannedOperation.ExecutedCount++;

            // Деактивация если достигнут лимит
            if (plannedOperation.MaxExecutions.HasValue &&
                plannedOperation.ExecutedCount >= plannedOperation.MaxExecutions.Value)
            {
                plannedOperation.IsActive = false;
                _logger.LogInformation("Плановый платеж {PlannedOperationId} деактивирован после достижения лимита выполнений", id);
            }

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Плановый платеж {PlannedOperationId} выполнен вручную пользователем {UserId}",
                id, currentUserId);

            return _mapper.Map<OperationReadDto>(operation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при ручном выполнении планового платежа {PlannedOperationId}", id);
            throw new CommonErrorException("Ошибка при выполнении планового платежа");
        }
    }

    /// <summary>
    /// Приостановка планового платежа
    /// </summary>
    public async Task<PlannedOperationStatusDto> PausePlannedOperationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var currentUserId = _currentUser.GetCurrentUserId();
        var plannedOperation = await _context.Set<PlannedOperation>()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == currentUserId && !x.IsDeleted, cancellationToken);

        if (plannedOperation == null)
        {
            throw new CommonErrorException("Плановый платеж не найден");
        }

        if (plannedOperation.IsPaused)
        {
            throw new CommonErrorException("Плановый платеж уже приостановлен");
        }

        plannedOperation.IsPaused = true;
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Плановый платеж {PlannedOperationId} приостановлен пользователем {UserId}",
            id, currentUserId);

        var result = _mapper.Map<PlannedOperationStatusDto>(plannedOperation);
        result.Message = "Плановый платеж успешно приостановлен";
        return result;
    }

    /// <summary>
    /// Возобновление планового платежа
    /// </summary>
    public async Task<PlannedOperationStatusDto> ResumePlannedOperationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var currentUserId = _currentUser.GetCurrentUserId();
        var plannedOperation = await _context.Set<PlannedOperation>()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == currentUserId && !x.IsDeleted, cancellationToken);

        if (plannedOperation == null)
        {
            throw new CommonErrorException("Плановый платеж не найден");
        }

        if (!plannedOperation.IsPaused)
        {
            throw new CommonErrorException("Плановый платеж не приостановлен");
        }

        if (!plannedOperation.IsActive)
        {
            throw new CommonErrorException("Нельзя возобновить неактивный плановый платеж");
        }

        plannedOperation.IsPaused = false;
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Плановый платеж {PlannedOperationId} возобновлен пользователем {UserId}",
            id, currentUserId);

        var result = _mapper.Map<PlannedOperationStatusDto>(plannedOperation);
        result.Message = "Плановый платеж успешно возобновлен";
        return result;
    }

    /// <summary>
    /// Предварительный просмотр будущих выполнений планового платежа
    /// </summary>
    public async Task<PlannedOperationPreviewListDto> GetPlannedOperationPreviewAsync(Guid id, int maxExecutions = 10, CancellationToken cancellationToken = default)
    {
        var currentUserId = _currentUser.GetCurrentUserId();
        var plannedOperation = await _context.Set<PlannedOperation>()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == currentUserId && !x.IsDeleted, cancellationToken);

        if (plannedOperation == null)
        {
            throw new CommonErrorException("Плановый платеж не найден");
        }

        var result = _mapper.Map<PlannedOperationPreviewListDto>(plannedOperation);
        result.Executions = GeneratePreviewExecutions(plannedOperation, maxExecutions);

        return result;
    }

    /// <summary>
    /// Генерация списка предварительного просмотра выполнений
    /// </summary>
    private List<PlannedOperationPreviewDto> GeneratePreviewExecutions(PlannedOperation plannedOperation, int maxExecutions)
    {
        var executions = new List<PlannedOperationPreviewDto>();
        var currentDate = plannedOperation.NextExecutionDate;
        int executionNumber = plannedOperation.ExecutedCount + 1;

        // Ограничиваем количество предварительного просмотра
        int remainingExecutions = plannedOperation.MaxExecutions.HasValue
            ? Math.Min(maxExecutions, plannedOperation.MaxExecutions.Value - plannedOperation.ExecutedCount)
            : maxExecutions;

        for (int i = 0; i < remainingExecutions; i++)
        {
            // Проверяем, не превышает ли дата окончания
            if (plannedOperation.EndDate.HasValue && currentDate > plannedOperation.EndDate.Value)
            {
                break;
            }

            executions.Add(new PlannedOperationPreviewDto
            {
                ExecutionDate = currentDate,
                Amount = plannedOperation.Amount,
                Description = plannedOperation.Description,
                ExecutionNumber = executionNumber + i
            });

            // Вычисляем следующую дату
            currentDate = CalculateNextExecutionDate(currentDate, plannedOperation.Frequency, plannedOperation.FrequencyType);
        }

        return executions;
    }

    /// <summary>
    /// Расчет следующей даты выполнения
    /// </summary>
    private static DateTimeOffset CalculateNextExecutionDate(DateTimeOffset currentDate, int frequency, FrequencyType frequencyType)
    {
        return frequencyType switch
        {
            FrequencyType.Daily => currentDate.AddDays(frequency),
            FrequencyType.Weekly => currentDate.AddDays(7 * frequency),
            FrequencyType.Monthly => currentDate.AddMonths(frequency),
            FrequencyType.Yearly => currentDate.AddYears(frequency),
            _ => throw new ArgumentException($"Неподдерживаемый тип частоты: {frequencyType}")
        };
    }
}

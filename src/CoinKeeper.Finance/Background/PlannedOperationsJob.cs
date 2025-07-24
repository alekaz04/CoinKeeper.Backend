using AutoMapper;
using CoinKeeper.Hangfire.AspNetCore;
using CoinKeeper.Infrastructure;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CoinKeeper.Finance;

public class PlannedOperationsJob : IHangfireRecurringJob
{
    public string CronExpression { get; } = Cron.Daily(1);
    public RecurringJobOptions? JobOptions { get; }

    private readonly IServiceProvider _serviceProvider;

    public PlannedOperationsJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Execute(CancellationToken token)
    {
        using var scope = _serviceProvider.CreateScope();
        var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DataContext>>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        var balanceService = scope.ServiceProvider.GetRequiredService<IAccountBalanceService>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<PlannedOperationsJob>>();

        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(token);

            var now = DateTimeOffset.UtcNow;

            // Получаем плановые операции, которые должны быть выполнены
            var plannedOperations = await context.Set<PlannedOperation>()
                .Include(x => x.Account)
                .Include(x => x.Category)
                .Where(x => x.IsActive && !x.IsDeleted && !x.IsPaused)
                .Where(x => x.StartDate <= now)
                .Where(x => x.EndDate == null || x.EndDate >= now)
                .Where(x => x.MaxExecutions == null || x.ExecutedCount < x.MaxExecutions)
                .Where(x => ShouldExecuteNow(x, now))
                .ToListAsync(cancellationToken: token);

            if (plannedOperations.Count > 0)
            {
                logger.LogInformation("Найдено {Count} плановых операций для выполнения", plannedOperations.Count);
            }

            var successCount = 0;
            var errorCount = 0;

            foreach (var plannedOperation in plannedOperations)
            {
                try
                {
                    await ExecutePlannedOperation(plannedOperation, context, mapper, balanceService, logger, token);
                    successCount++;
                }
                catch (Exception ex)
                {
                    errorCount++;
                    logger.LogError(ex, "Ошибка при выполнении планового платежа {PlannedOperationId} ({Name})",
                        plannedOperation.Id, plannedOperation.Name);

                    // Продолжаем выполнение других операций даже при ошибке
                    continue;
                }
            }

            if (plannedOperations.Count > 0)
            {
                await context.SaveChangesAsync(token);
                logger.LogInformation("Выполнение плановых операций завершено. Успешно: {SuccessCount}, Ошибок: {ErrorCount}",
                    successCount, errorCount);
            }
        }
        catch (Exception ex)
        {
            var errorLogger = scope.ServiceProvider.GetRequiredService<ILogger<PlannedOperationsJob>>();
            errorLogger.LogError(ex, "Критическая ошибка при выполнении задачи плановых операций");
            throw;
        }
    }

    /// <summary>
    /// Проверяет, должна ли операция быть выполнена сейчас с учетом времени выполнения
    /// </summary>
    private static bool ShouldExecuteNow(PlannedOperation plannedOperation, DateTimeOffset now)
    {
        var executionDateTime = plannedOperation.NextExecutionDate.Date.Add(plannedOperation.ScheduledTime);
        return executionDateTime <= now;
    }

    private async Task ExecutePlannedOperation(
        PlannedOperation plannedOperation,
        DataContext context,
        IMapper mapper,
        IAccountBalanceService balanceService,
        ILogger logger,
        CancellationToken token)
    {
        logger.LogInformation("Выполнение планового платежа {PlannedOperationId} ({Name}) для пользователя {UserId}",
            plannedOperation.Id, plannedOperation.Name, plannedOperation.UserId);

        // Создание операции с учетом запланированного времени
        var operation = mapper.Map<Operation>(plannedOperation);
        var executionDateTime = plannedOperation.NextExecutionDate.Date.Add(plannedOperation.ScheduledTime);
        operation.OperationTime = executionDateTime;
        operation.Description = $"Автоматическое выполнение: {plannedOperation.Name}";

        // Применение операции к балансу
        await balanceService.ApplyOperationToBalance(plannedOperation.UserId, operation, token);

        // Обновление счетчика выполнений
        plannedOperation.ExecutedCount++;

        // Расчет следующей даты выполнения
        plannedOperation.NextExecutionDate = CalculateNextExecutionDate(plannedOperation);

        // Деактивация если достигнут лимит выполнений
        if (plannedOperation.MaxExecutions.HasValue &&
            plannedOperation.ExecutedCount >= plannedOperation.MaxExecutions.Value)
        {
            plannedOperation.IsActive = false;
            logger.LogInformation("Плановый платеж {PlannedOperationId} деактивирован после достижения лимита выполнений ({MaxExecutions})",
                plannedOperation.Id, plannedOperation.MaxExecutions.Value);
        }

        // Деактивация если достигнута дата окончания
        if (plannedOperation.EndDate.HasValue && plannedOperation.NextExecutionDate > plannedOperation.EndDate.Value)
        {
            plannedOperation.IsActive = false;
            logger.LogInformation("Плановый платеж {PlannedOperationId} деактивирован после достижения даты окончания",
                plannedOperation.Id);
        }

        logger.LogInformation("Плановый платеж {PlannedOperationId} успешно выполнен. Следующее выполнение: {NextExecution}",
            plannedOperation.Id, plannedOperation.NextExecutionDate);
    }

    private DateTimeOffset CalculateNextExecutionDate(PlannedOperation plannedOperation)
    {
        var nextDate = plannedOperation.FrequencyType switch
        {
            FrequencyType.Daily => plannedOperation.NextExecutionDate.AddDays(plannedOperation.Frequency),
            FrequencyType.Weekly => plannedOperation.NextExecutionDate.AddDays(7 * plannedOperation.Frequency),
            FrequencyType.Monthly => plannedOperation.NextExecutionDate.AddMonths(plannedOperation.Frequency),
            FrequencyType.Yearly => plannedOperation.NextExecutionDate.AddYears(plannedOperation.Frequency),
            _ => throw new ArgumentException($"Неподдерживаемый тип частоты: {plannedOperation.FrequencyType}")
        };

        // Сохраняем время выполнения
        return nextDate.Date.Add(plannedOperation.ScheduledTime);
    }
}

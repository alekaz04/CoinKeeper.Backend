using AutoMapper;
using CoinKeeper.Hangfire.AspNetCore;
using CoinKeeper.Infrastructure;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoinKeeper.Finance;

public class PlannedOperationsJob : IHangfireRecurringJob
{
    public string CronExpression { get; } = Cron.Minutely(); // Каждую минуту для более точного выполнения
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

        await using var context = await contextFactory.CreateDbContextAsync(token);

        var now = DateTimeOffset.Now;

        var plannedOperations = await context.Set<PlannedOperation>()
            .Include(x => x.Account)
            .Include(x => x.Category)
            .Where(x => x.NextExecutionDate <= now)
            .Where(x => x.IsActive && !x.IsDeleted && !x.IsPaused)
            .Where(x => x.StartDate <= now)
            .Where(x => x.EndDate == null || x.EndDate >= now)
            .Where(x => x.MaxExecutions == null || x.ExecutedCount < x.MaxExecutions)
            .ToListAsync(cancellationToken: token);

        foreach (var plannedOperation in plannedOperations)
        {
            await ExecutePlannedOperation(plannedOperation, context, mapper, balanceService, token);
        }

        await context.SaveChangesAsync(token);
    }

    private async Task ExecutePlannedOperation(
        PlannedOperation plannedOperation,
        DataContext context,
        IMapper mapper,
        IAccountBalanceService balanceService,
        CancellationToken token)
    {
        // Создание операции
        var operation = mapper.Map<Operation>(plannedOperation);
        await balanceService.ApplyOperationToBalance(plannedOperation.UserId, operation, token);

        // Обновление счетчика
        plannedOperation.ExecutedCount++;

        // Расчет следующей даты выполнения
        plannedOperation.NextExecutionDate = CalculateNextExecutionDate(plannedOperation);

        // Деактивация если достигнут лимит
        if (plannedOperation.MaxExecutions.HasValue &&
            plannedOperation.ExecutedCount >= plannedOperation.MaxExecutions.Value)
        {
            plannedOperation.IsActive = false;
        }
    }

    private DateTimeOffset CalculateNextExecutionDate(PlannedOperation plannedOperation)
    {
        return plannedOperation.FrequencyType switch
        {
            FrequencyType.Daily => plannedOperation.NextExecutionDate.AddDays(plannedOperation.Frequency),
            FrequencyType.Weekly => plannedOperation.NextExecutionDate.AddDays(7 * plannedOperation.Frequency),
            FrequencyType.Monthly => plannedOperation.NextExecutionDate.AddMonths(plannedOperation.Frequency),
            FrequencyType.Yearly => plannedOperation.NextExecutionDate.AddYears(plannedOperation.Frequency),
            _ => throw new ArgumentException($"Неподдерживаемый тип частоты: {plannedOperation.FrequencyType}")
        };
    }
}

using CoinKeeper.Hangfire.AspNetCore;
using CoinKeeper.Infrastructure;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace CoinKeeper.Finance;

public class PlannedOperationsJob : IHangfireRecurringJob
{
    public string CronExpression { get; } = Cron.Daily(1, 15);
    public RecurringJobOptions? JobOptions { get; }

    private readonly IDbContextFactory<DataContext> _contextFactory;


    public PlannedOperationsJob(IDbContextFactory<DataContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task Execute(CancellationToken token)
    {
        var context = await _contextFactory.CreateDbContextAsync(token);

        var plannedOperations = await context.Set<PlannedOperation>()
            .Include(p => p.User)
            .Include(x => x.Account)
            .Where(x => x.NextExecutionDate.DayOfYear == DateTimeOffset.Now.DayOfYear)
            .ToListAsync(cancellationToken: token);

        foreach (var plannedOperation in plannedOperations)
        {
            var operation = new Operation()
            {
                Id = Guid.NewGuid(),
                OperationTime = DateTimeOffset.UtcNow,
                Amount = plannedOperation.Amount,
                Description = "",
                OperationType = plannedOperation.OperationType,

            };
        }


    }
}

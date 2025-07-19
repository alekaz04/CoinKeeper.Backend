using AutoMapper;
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
    private readonly IMapper _mapper;
    private readonly IAccountBalanceService _balanceService;

    public PlannedOperationsJob(IDbContextFactory<DataContext> contextFactory, IMapper mapper, IAccountBalanceService balanceService)
    {
        _contextFactory = contextFactory;
        _mapper = mapper;
        _balanceService = balanceService;
    }

    public async Task Execute(CancellationToken token)
    {
        var context = await _contextFactory.CreateDbContextAsync(token);

        var plannedOperations = await context.Set<PlannedOperation>()
            .Include(x => x.Account)
            .Where(x => x.NextExecutionDate.DayOfYear == DateTimeOffset.Now.DayOfYear)
            .Where(x => x.IsActive && !x.IsDeleted)
            .ToListAsync(cancellationToken: token);

        foreach (var plannedOperation in plannedOperations)
        {
            var operation = _mapper.Map<Operation>(plannedOperation);
            await _balanceService.ApplyOperationToBalance(plannedOperation.UserId, operation, token);

            plannedOperation.NextExecutionDate = plannedOperation.NextExecutionDate.Add(plannedOperation.ScheduledTime);
        }

        await context.SaveChangesAsync(token);
    }
}

using CoinKeeper.Hangfire.AspNetCore;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CoinKeeper.Finance;

public class RecalculationAllUsersBalanceJob : IHangfireRecurringJob
{
    private readonly ILogger<RecalculationAllUsersBalanceJob> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public string CronExpression { get; } = Cron.Daily(0);
    public RecurringJobOptions? JobOptions { get; } = new();

    public RecalculationAllUsersBalanceJob(ILogger<RecalculationAllUsersBalanceJob> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Execute(CancellationToken token)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        _logger.LogInformation("Recalculating all users balance");

        var balanceService = scope.ServiceProvider.GetRequiredService<IAccountBalanceService>();

        await balanceService.UpdateAllAccountBalances(token);
    }
}

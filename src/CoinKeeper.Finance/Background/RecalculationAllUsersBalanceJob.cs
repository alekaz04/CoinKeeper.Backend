using CoinKeeper.Hangfire.AspNetCore;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CoinKeeper.Finance;

/// <summary>
/// Задача по перерасчёту баланса всех пользователей
/// </summary>
public class RecalculationAllUsersBalanceJob : IHangfireRecurringJob
{
    /// <inheritdoc cref="ILogger{T}"/>
    private readonly ILogger<RecalculationAllUsersBalanceJob> _logger;

    /// <inheritdoc cref="IServiceScopeFactory"/>
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <inheritdoc />
    public string CronExpression { get; } = Cron.Daily(0);

    /// <inheritdoc />
    public RecurringJobOptions? JobOptions { get; } = new();

    public RecalculationAllUsersBalanceJob(ILogger<RecalculationAllUsersBalanceJob> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    /// <inheritdoc />
    public async Task Execute(CancellationToken token)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        _logger.LogInformation("Recalculating all users balance");

        var balanceService = scope.ServiceProvider.GetRequiredService<IAccountBalanceService>();

        await balanceService.UpdateAllAccountBalances(token);
    }
}

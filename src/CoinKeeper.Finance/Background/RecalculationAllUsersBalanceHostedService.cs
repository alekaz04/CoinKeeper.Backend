using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoinKeeper.Finance;

public class RecalculationAllUsersBalanceHostedService : BackgroundService
{
    private readonly ILogger<RecalculationAllUsersBalanceHostedService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RecalculationAllUsersBalanceHostedService(ILogger<RecalculationAllUsersBalanceHostedService> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromHours(24));

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            using var scope = _serviceScopeFactory.CreateScope();
            _logger.LogInformation("Recalculating all users balance");
            var balanceService = scope.ServiceProvider.GetRequiredService<IAccountBalanceService>();
            await balanceService.UpdateAllAccountBalances(stoppingToken);
        }
    }
}

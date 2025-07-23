namespace CoinKeeper.Finance;

public interface IBalanceHandler
{
    public Task<BalanceUserDto> GetCurrentUserBalance(CancellationToken token);
}

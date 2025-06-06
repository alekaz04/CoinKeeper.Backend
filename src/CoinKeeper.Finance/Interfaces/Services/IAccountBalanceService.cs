namespace CoinKeeper.Finance;

public interface IAccountBalanceService
{
    public Task ApplyOperationToBalance(Operation operation, CancellationToken token);
    public Task UpdateAllAccountBalances(CancellationToken cancellationToken);
    public Task UpdateAccountBalance(Guid accountId, CancellationToken token);
}

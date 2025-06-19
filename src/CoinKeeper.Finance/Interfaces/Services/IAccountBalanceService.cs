namespace CoinKeeper.Finance;

public interface IAccountBalanceService
{
    public Task ApplyOperationToBalance(Operation operation, CancellationToken token);

    /// <summary>
    /// Обновляет баланс У ВСЕХ пользователей
    /// </summary>
    /// <param name="cancellationToken">Токен отмены запроса</param>
    /// <remarks>Вызывать ТОЛЬКО в бэкграунд сервисе, тк могут быть проблемы с производительностью</remarks>
    public Task UpdateAllAccountBalances(CancellationToken cancellationToken);
    public Task UpdateAccountBalance(Guid accountId, CancellationToken token);
}

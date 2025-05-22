using CoinKeeper.Authentication.Domain;
using CoinKeeper.Common;
using CoinKeeper.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CoinKeeper.Finance;

public class AccountBalanceService : IAccountBalanceService
{
    private readonly DataContext _context;
    private readonly ICurrentUser _currentUser;

    public AccountBalanceService(DataContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task ApplyOperationToBalance(Operation operation, CancellationToken token)
    {
        var user = _currentUser.GetCurrentUserId();

        var account = await _context.Set<Account>()
            .FirstOrDefaultAsync(a => a.Id == operation.AccountId && a.UserId == user, token);

        if (account == null)
        {
            throw new CommonErrorException("Счет не найден");
        }

        if (operation.OperationType == OperationType.Income)
        {
            account.Balance += operation.Amount;
        }
        else
        {
            account.Balance -= operation.Amount;
        }

        await _context.SaveChangesAsync(token);
    }

    /// <summary>
    /// Обновляет баланс У ВСЕХ пользователей
    /// </summary>
    /// <param name="cancellationToken">Токен отмены запроса</param>
    /// <remarks>Вызывать ТОЛЬКО в бэкграунд сервисе, тк могут быть проблемы с производительностью</remarks>
    public async Task UpdateAllAccountBalances(CancellationToken cancellationToken)
    {
        var userIds = await _context.Set<User>()
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        foreach (var userId in userIds)
        {
            var userAccounts = await _context.Set<Account>()
                .Where(x => x.UserId == userId && !x.IsDeleted)
                .ToListAsync(cancellationToken);

            foreach (var accountId in userAccounts)
            {
                await UpdateAccountBalance(accountId, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }


    /// <summary>
    /// Обновить баланс ВСЕХ счетов у пользователя
    /// </summary>
    /// <param name="account">Счёт</param>
    /// <param name="token">Токен отмены запроса</param>
    private async Task UpdateAccountBalance(Account account, CancellationToken token)
    {
        decimal incomeSum = await _context.Set<Operation>()
            .Where(o => o.AccountId == account.Id && !o.IsDeleted && o.OperationType == OperationType.Income)
            .SumAsync(o => o.Amount, token);

        decimal expenseSum = await _context.Set<Operation>()
            .Where(o => o.AccountId == account.Id && !o.IsDeleted && o.OperationType == OperationType.Expense)
            .SumAsync(o => o.Amount, token);

        account.Balance = incomeSum - expenseSum;
    }


    public async Task UpdateAccountBalance(Guid accountId, CancellationToken token)
    {
        var user = _currentUser.GetCurrentUserId();

        var account = await _context.Set<Account>()
            .FirstOrDefaultAsync(a => a.Id == accountId && a.UserId == user, token);

        if (account == null)
        {
            throw new CommonErrorException("Счет не найден");
        }

        await UpdateAccountBalance(account, token);
        await _context.SaveChangesAsync(token);
    }
}

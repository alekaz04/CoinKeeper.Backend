using CoinKeeper.Common;
using Microsoft.AspNetCore.Mvc;

namespace CoinKeeper.Finance;

public class AccountController : AbstractCrudController<Account, AccountReadDto, AccountCreateDto, AccountUpdateDto>
{
    private readonly IBalanceHandler _balanceHandler;

    public AccountController(IBalanceHandler balanceHandler, AbstractCrudHandler<Account, AccountReadDto, AccountCreateDto, AccountUpdateDto> handler) : base(handler)
    {
        _balanceHandler = balanceHandler;
    }

    [HttpGet("balance")]
    public async Task<BalanceUserDto> GetBalanceForUser(CancellationToken token)
    {
        return await _balanceHandler.GetCurrentUserBalance(token);
    }
}

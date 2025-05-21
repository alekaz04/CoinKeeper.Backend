using CoinKeeper.Common;

namespace CoinKeeper.Finance;

public class AccountController : AbstractCrudController<Account, AccountReadDto, AccountCreateDto>
{
    public AccountController(AbstractCrudHandler<Account, AccountReadDto, AccountCreateDto> handler) : base(handler)
    {

    }
}

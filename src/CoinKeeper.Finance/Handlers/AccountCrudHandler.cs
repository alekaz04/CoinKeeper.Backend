using AutoMapper;
using CoinKeeper.Authentication.Domain;
using CoinKeeper.Common;
using CoinKeeper.Infrastructure;
using FluentValidation;

namespace CoinKeeper.Finance;

/// <summary>
/// Круд хэндлер для сущности <see cref="Account"/>
/// </summary>
public class AccountCrudHandler : AbstractCrudHandler<Account, AccountReadDto, AccountCreateDto>
{
    public AccountCrudHandler(DataContext context, IMapper mapper, IValidator<AccountCreateDto> validator, ICurrentUser currentUser) : base(context, mapper, validator, currentUser)
    {
    }
}

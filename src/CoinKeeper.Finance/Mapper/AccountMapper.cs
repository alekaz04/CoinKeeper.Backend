using AutoMapper;
using CoinKeeper.Common;

namespace CoinKeeper.Finance;

public class AccountMapper : Profile
{
    public AccountMapper()
    {
        CreateMap<AccountCreateDto, Account>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
            .ForMember(x => x.UserId, opt => opt.MapFrom<CurrentUserResolver>());

        CreateMap<Account, AccountReadDto>();

        CreateMap<AccountUpdateDto, Account>();
        CreateMap<Account, AccountBalanceDto>();
    }
}

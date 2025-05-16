using AutoMapper;
using CoinKeeper.Authentication.Domain;

namespace CoinKeeper.Authentication.Mapper;

/// <summary>
/// Маппинги сущности <see cref="User"/>
/// </summary>
public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<RequestUserDto, User>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
            .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => DateTimeOffset.UtcNow))
            .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(x => DateTimeOffset.UtcNow));

    }
}

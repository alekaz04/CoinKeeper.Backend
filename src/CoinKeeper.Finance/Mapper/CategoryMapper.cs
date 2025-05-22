using AutoMapper;
using CoinKeeper.Common;

namespace CoinKeeper.Finance;

/// <summary>
/// Профиль маппера для сущности <see cref="Category"/>
/// </summary>
public class CategoryMapper : Profile
{
    public CategoryMapper()
    {
        CreateMap<CategoryCreateDto, Category>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
            .ForMember(x => x.UserId, opt => opt.MapFrom<CurrentUserResolver>());

        CreateMap<Category, CategoryReadDto>();

        CreateMap<CategoryUpdateDto, Category>();
    }
}

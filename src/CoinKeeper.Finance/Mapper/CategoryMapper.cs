using AutoMapper;

namespace CoinKeeper.Finance;

public class CategoryMapper : Profile
{
    public CategoryMapper()
    {
        CreateMap<CategoryCreateDto, Category>()
            .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => DateTimeOffset.UtcNow))
            .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(x => DateTimeOffset.UtcNow))
            .ForMember(x => x.Id, opt => opt.MapFrom(x => Guid.NewGuid()));

        CreateMap<Category, CategoryReadDto>();

    }
}

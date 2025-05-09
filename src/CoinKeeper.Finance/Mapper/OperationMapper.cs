using AutoMapper;

namespace CoinKeeper.Finance;

public class OperationMapper : Profile
{
    public OperationMapper()
    {
        CreateMap<OperationCreateDto, Operation>()
            .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => DateTimeOffset.UtcNow))
            .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(x => DateTimeOffset.UtcNow))
            .ForMember(x => x.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
            .ForMember(x => x.UserId, opt => opt.MapFrom<CurrentUserResolver>());

        CreateMap<Operation, OperationReadDto>();
    }
}

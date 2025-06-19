using AutoMapper;
using CoinKeeper.Common;

namespace CoinKeeper.Finance;

public class PlannedOperationMapper : Profile
{
    public PlannedOperationMapper()
    {
        CreateMap<PlannedOperationCreateDto, PlannedOperation>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
            .ForMember(x => x.UserId, opt => opt.MapFrom<CurrentUserResolver>());

        CreateMap<PlannedOperation, PlannedOperationReadDto>();

        CreateMap<PlannedOperationUpdateDto, PlannedOperation>();
    }
}

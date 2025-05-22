using AutoMapper;
using CoinKeeper.Common;

namespace CoinKeeper.Finance;

public class OperationMapper : Profile
{
    public OperationMapper()
    {
        CreateMap<OperationCreateDto, Operation>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
            .ForMember(x => x.UserId, opt => opt.MapFrom<CurrentUserResolver>());

        CreateMap<Operation, OperationReadDto>().ReverseMap();

        CreateMap<OperationUpdateDto, Operation>();
    }
}

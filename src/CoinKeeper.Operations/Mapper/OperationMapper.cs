using AutoMapper;
using CoinKeeper.Operations.Dto;

namespace CoinKeeper.Operations.Mapper;

public class OperationMapper : Profile
{
    public OperationMapper()
    {
        CreateMap<OperationCreateDto, Operation>()
            .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => DateTimeOffset.UtcNow))
            .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(x => DateTimeOffset.UtcNow))
            .ForMember(x => x.Id, opt => opt.MapFrom(x => Guid.NewGuid()));

        CreateMap<Operation, OperationReadDto>();
    }
}

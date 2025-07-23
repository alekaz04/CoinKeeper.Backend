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

        // Маппинг для конвертации планового платежа в операцию
        CreateMap<PlannedOperation, Operation>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
            .ForMember(x => x.OperationTime, opt => opt.MapFrom(x => DateTimeOffset.Now))
            .ForMember(x => x.Description, opt => opt.MapFrom(x => $"Автоматическое выполнение: {x.Name}"));

        // Маппинг для статуса планового платежа
        CreateMap<PlannedOperation, PlannedOperationStatusDto>()
            .ForMember(x => x.Message, opt => opt.Ignore());

        // Маппинг для предварительного просмотра
        CreateMap<PlannedOperation, PlannedOperationPreviewListDto>()
            .ForMember(x => x.Executions, opt => opt.Ignore())
            .ForMember(x => x.TotalExecutions, opt => opt.MapFrom(x => x.MaxExecutions));
    }
}

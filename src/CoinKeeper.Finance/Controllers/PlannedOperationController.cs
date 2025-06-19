using CoinKeeper.Common;

namespace CoinKeeper.Finance;

/// <summary>
/// Контроллер для сущности <see cref="PlannedOperation"/>
/// </summary>
public class PlannedOperationController : AbstractCrudController<PlannedOperation, PlannedOperationReadDto, PlannedOperationCreateDto, PlannedOperationUpdateDto>
{
    public PlannedOperationController(AbstractCrudHandler<PlannedOperation, PlannedOperationReadDto, PlannedOperationCreateDto, PlannedOperationUpdateDto> handler) : base(handler)
    {
    }
}

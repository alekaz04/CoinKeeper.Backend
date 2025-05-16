using CoinKeeper.Common;

namespace CoinKeeper.Finance;

/// <summary>
/// Контроллер для сущности <see cref="Operation"/>
/// </summary>
public class OperationController : AbstractCrudController<Operation, OperationReadDto, OperationCreateDto>
{
    public OperationController(AbstractCrudHandler<Operation, OperationReadDto, OperationCreateDto> handler) : base(handler)
    {
    }
}

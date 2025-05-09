using CoinKeeper.Common;

namespace CoinKeeper.Finance;

public class OperationController : AbstractCrudController<Operation, OperationReadDto, OperationCreateDto>
{
    public OperationController(AbstractCrudHandler<Operation, OperationReadDto, OperationCreateDto> handler) : base(handler)
    {
    }
}

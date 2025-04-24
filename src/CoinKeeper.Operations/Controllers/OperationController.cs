using CoinKeeper.Common;
using CoinKeeper.Operations.Dto;
using CoinKeeper.Operations.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace CoinKeeper.Operations;

public class OperationController : CommonApiController
{
    private readonly OperationsCrudHandler _handler;

    public OperationController(OperationsCrudHandler handler)
    {
        _handler = handler;
    }

    [HttpPost("/create")]
    public async Task<Guid> CreateOperation([FromBody] OperationCreateDto dto, CancellationToken cancellationToken)
    {
        return await _handler.CreateOperation(dto, cancellationToken);
    }

    [HttpGet("/get/{id}")]
    public async Task<OperationReadDto> GetOperation(Guid id, CancellationToken cancellationToken)
    {
        return await _handler.GetOperationById(id, cancellationToken);
    }
}

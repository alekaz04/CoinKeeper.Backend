using CoinKeeper.Authentication.Domain;
using CoinKeeper.Common.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CoinKeeper.Common;

public class AbstractCrudController<TEntity, TReadDto, TCreateDto> : CommonApiController
    where TEntity : class, IBaseEntity, IUserSpecifiedEntity
{
    private readonly AbstractCrudHandler<TEntity, TReadDto, TCreateDto> _handler;

    public AbstractCrudController(AbstractCrudHandler<TEntity, TReadDto, TCreateDto> handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<Guid> Create([FromBody] TCreateDto dto, CancellationToken cancellationToken)
    {
        return await _handler.Create(dto, cancellationToken);
    }

    [HttpGet("{id:guid}")]
    public async Task<TReadDto> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await _handler.GetById(id, cancellationToken);
    }

    [HttpGet]
    public async Task<IEnumerable<TReadDto>> GetAll(CancellationToken cancellationToken)
    {
        return await _handler.GetAllForUser(cancellationToken);
    }

    [HttpDelete("{id:guid}")]
    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        await _handler.Delete(id, cancellationToken);
    }
}

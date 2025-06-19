using CoinKeeper.Authentication.Domain;
using CoinKeeper.Common.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CoinKeeper.Common;

/// <summary>
/// Базовый круд контроллер
/// </summary>
/// <typeparam name="TEntity">Исходная сущность</typeparam>
/// <typeparam name="TEntityDto">Дто сущности</typeparam>
/// <typeparam name="TCreateDto">Дто создания сущности</typeparam>
/// <typeparam name="TUpdateDto">Дто обновления сущности</typeparam>
public class AbstractCrudController<TEntity, TEntityDto, TCreateDto, TUpdateDto> : CommonApiController
    where TEntity : class, IBaseEntity, IUserSpecifiedEntity
{
    /// <inheritdoc cref="AbstractCrudHandler{TEntity, TEntityDto, TCreateDto}"/>
    private readonly AbstractCrudHandler<TEntity, TEntityDto, TCreateDto, TUpdateDto> _handler;

    public AbstractCrudController(AbstractCrudHandler<TEntity, TEntityDto, TCreateDto, TUpdateDto> handler)
    {
        _handler = handler;
    }

    /// <summary>
    /// Создать <see cref="TEntity"/>
    /// </summary>
    /// <param name="dto">Дто создания <see cref="TEntity"/></param>
    /// <param name="cancellationToken">Токен отмены запроса</param>
    /// <returns>Идентификатор созданной сущности</returns>
    [HttpPost]
    public async Task<Guid> Create([FromBody] TCreateDto dto, CancellationToken cancellationToken)
    {
        return await _handler.Create(dto, cancellationToken);
    }

    /// <summary>
    /// Получить <see cref="TEntityDto"/> по идентификатору
    /// </summary>
    /// <param name="id">Идентфиикатор</param>
    /// <param name="cancellationToken">Токен отмены запроса</param>
    /// <returns>Дто сущности <see cref="TEntityDto"/></returns>
    [HttpGet("{id:guid}")]
    public async Task<TEntityDto> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await _handler.GetById(id, cancellationToken);
    }

    /// <summary>
    /// Получить все сущности данного пользователя
    /// </summary>
    /// <param name="cancellationToken">Токен отмены запроса</param>
    [HttpGet]
    public async Task<IEnumerable<TEntityDto>> GetAll(CancellationToken cancellationToken)
    {
        return await _handler.GetAllForUser(cancellationToken);
    }

    /// <summary>
    /// Обновить сущность
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <param name="entityDto">Дто сущности</param>
    /// <param name="cancellationToken">Токен отмены запроса</param>
    [HttpPut("{id:guid}")]
    public async Task Update(Guid id, TUpdateDto entityDto, CancellationToken cancellationToken)
    {
        await _handler.Update(id, entityDto, cancellationToken);
    }

    /// <summary>
    /// Мягко удалить сущность по идентфикатору
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <param name="cancellationToken"></param>
    [HttpDelete("{id:guid}")]
    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        await _handler.Delete(id, cancellationToken);
    }
}

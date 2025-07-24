using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoinKeeper.Authentication.Domain;
using CoinKeeper.Common.Domain;
using CoinKeeper.Common.Domain.Pagination;
using CoinKeeper.Infrastructure;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CoinKeeper.Common;

/// <summary>
/// Базовый круд хэндлер
/// </summary>
/// <typeparam name="TEntity">Исходная сущность</typeparam>
/// <typeparam name="TEntityDto">Дто сущности</typeparam>
/// <typeparam name="TCreateDto">Дто создания сущности</typeparam>
/// <typeparam name="TUpdateDto">Дто обновления сущности</typeparam>
public abstract class AbstractCrudHandler<TEntity, TEntityDto, TCreateDto, TUpdateDto>
    where TEntity : class, IBaseEntity, IUserSpecifiedEntity
{
    /// <inheritdoc cref="DataContext"/>
    private readonly DataContext _context;

    /// <inheritdoc cref="IMapper"/>
    private readonly IMapper _mapper;

    /// <inheritdoc cref="IValidator{T}"/>
    private readonly IValidator<TCreateDto> _validator;

    /// <inheritdoc cref="ICurrentUser"/>
    private readonly ICurrentUser _currentUser;

    protected AbstractCrudHandler(DataContext context,
        IMapper mapper,
        IValidator<TCreateDto> validator,
        ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Создать <see cref="TEntity"/>
    /// </summary>
    /// <param name="createDto">Дто создания <see cref="TEntity"/></param>
    /// <param name="token">Токен отмены запроса</param>
    /// <returns>Идентификатор созданной сущности</returns>
    public virtual async Task<Guid> Create(TCreateDto createDto, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(createDto, token);

        var enitity = _mapper.Map<TEntity>(createDto);

        await _context.AddAsync(enitity, token);
        await _context.SaveChangesAsync(token);

        return enitity.Id;
    }

    /// <summary>
    /// Получить <see cref="TEntityDto"/> по идентификатору
    /// </summary>
    /// <param name="id">Идентфиикатор</param>
    /// <param name="token">Токен отмены запроса</param>
    /// <returns>Дто сущности <see cref="TEntityDto"/></returns>
    public virtual async Task<TEntityDto> GetById(Guid id, CancellationToken token)
    {
        var entityDto = await _context.Set<TEntity>()
            .AsNoTracking()
            .Where(x => x.Id == id && !x.IsDeleted)
            .ProjectTo<TEntityDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(token);

        if (entityDto is null)
        {
            throw new CommonErrorException("Entity not found");
        }

        return entityDto;
    }

    /// <summary>
    /// Получить все сущности данного пользователя
    /// </summary>
    /// <param name="pagination">Параметры пагинации</param>
    /// <param name="token">Токен отмены запроса</param>
    public virtual async Task<PagedResult<TEntityDto>> GetAllForUser(PaginationRequest pagination, CancellationToken token)
    {
        var currentUserId = _currentUser.GetCurrentUserId();

        var entitiesDto = await _context.Set<TEntity>()
            .AsNoTracking()
            .Where(x => x.UserId == currentUserId && !x.IsDeleted)
            .OrderByDescending(x => x.UpdatedAt)
            .Skip(pagination.Skip)
            .Take(pagination.Take)
            .ProjectTo<TEntityDto>(_mapper.ConfigurationProvider)
            .ToListAsync(token);

        int count = await _context.Set<TEntity>()
            .AsNoTracking()
            .Where(x => x.UserId == currentUserId && !x.IsDeleted)
            .CountAsync(token);

        var result = new PagedResult<TEntityDto>()
        {
            PageNumber = pagination.Skip / pagination.Take,
            PageSize = pagination.Take,
            TotalCount = count,
            Items = entitiesDto
        };

        return result;
    }

    /// <summary>
    /// Обновить сущность
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <param name="entityDto">Дто сущности</param>
    /// <param name="token">Токен отмены запроса</param>
    public virtual async Task Update(Guid id, TUpdateDto entityDto, CancellationToken token)
    {
        var currentUserId = _currentUser.GetCurrentUserId();

        var entity = await _context.Set<TEntity>()
            .Where(x => x.UserId == currentUserId && !x.IsDeleted && x.Id == id)
            .FirstOrDefaultAsync(token);

        if (entity is null)
        {
            throw new CommonErrorException($"Сущность {typeof(TEntity).Name} с идентификатором {id} не найдена");
        }

        _mapper.Map(entityDto, entity);
        await _context.SaveChangesAsync(token);
    }

    public virtual async Task Delete(Guid id, CancellationToken token)
    {
        var currentUserId = _currentUser.GetCurrentUserId();
        var entity = await _context.Set<TEntity>()
            .Where(x => x.Id == id && x.UserId == currentUserId && !x.IsDeleted)
            .FirstOrDefaultAsync(token);

        if (entity is null)
        {
            throw new CommonErrorException($"Сущность {typeof(TEntity).Name} с идентификатором {id} не найдена");
        }

        entity.IsDeleted = true;

        await _context.SaveChangesAsync(token);
    }
}

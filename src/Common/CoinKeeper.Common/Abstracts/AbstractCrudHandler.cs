using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoinKeeper.Authentication.Domain;
using CoinKeeper.Common.Domain;
using CoinKeeper.Infrastructure;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CoinKeeper.Common;

public abstract class AbstractCrudHandler<TEntity, TReadDto, TCreateDto>
    where TEntity : class, IBaseEntity, IUserSpecifiedEntity
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<TCreateDto> _validator;
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

    public virtual async Task<Guid> Create(TCreateDto entity, CancellationToken token)
    {
        _validator.ValidateAndThrow(entity);

        var contextEntity = _mapper.Map<TEntity>(entity);

        await _context.AddAsync(contextEntity, token);
        await _context.SaveChangesAsync(token);

        return contextEntity.Id;
    }

    public virtual async Task<TReadDto> GetById(Guid id, CancellationToken token)
    {
        var entityDto = await _context.Set<TEntity>()
            .AsNoTracking()
            .Where(x => x.Id == id && !x.IsDeleted)
            .ProjectTo<TReadDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(token);

        if (entityDto is null)
        {
            throw new CommonErrorException("Entity not found");
        }

        return entityDto;
    }

    public virtual async Task<IEnumerable<TReadDto>> GetAllForUser(CancellationToken token)
    {
        var currentUserId = _currentUser.GetCurrentUserId();

        var entityDtos = await _context.Set<TEntity>()
            .AsNoTracking()
            .Where(x => x.UserId == currentUserId && !x.IsDeleted)
            .ProjectTo<TReadDto>(_mapper.ConfigurationProvider)
            .ToListAsync(token);
        return entityDtos;
    }

    public virtual async Task Delete(Guid id, CancellationToken token)
    {
        var currentUserId = _currentUser.GetCurrentUserId();
        var entity = await _context.Set<TEntity>()
            .Where(x => x.Id == id && x.UserId == currentUserId && !x.IsDeleted)
            .FirstOrDefaultAsync(token);

        if (entity is null)
        {
            throw new CommonErrorException("Entity not found");
        }
        entity.IsDeleted = true;

        await _context.SaveChangesAsync(token);
    }
}

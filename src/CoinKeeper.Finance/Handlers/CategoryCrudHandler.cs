using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoinKeeper.Common;
using CoinKeeper.Infrastructure;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CoinKeeper.Finance;

public class CategoryCrudHandler
{
    private readonly IValidator<ICategory> _validator;
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public CategoryCrudHandler(IValidator<ICategory> validator, IMapper mapper, DataContext context)
    {
        _validator = validator;
        _mapper = mapper;
        _context = context;
    }

    public async Task<Guid> CreateCategory(CategoryCreateDto category, CancellationToken token)
    {
        _validator.ValidateAndThrow(category);

        var categoryEntity = _mapper.Map<Category>(category);

        await _context.AddAsync(categoryEntity, token);
        await _context.SaveChangesAsync(token);

        return categoryEntity.Id;
    }

    public async Task<CategoryReadDto> GetCategoryById(Guid id, CancellationToken token)
    {
        var category = await _context.Set<Category>()
            .AsNoTracking()
            .Where(x => x.Id == id)
            .ProjectTo<CategoryReadDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(token);

        if (category is null)
        {
            throw new CommonErrorException("Category not found");
        }

        return category;
    }
}

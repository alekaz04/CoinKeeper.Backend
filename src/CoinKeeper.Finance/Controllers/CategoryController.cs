using CoinKeeper.Common;

namespace CoinKeeper.Finance;

/// <summary>
/// Контроллер для сущности <see cref="Category"/>
/// </summary>
public class CategoryController : AbstractCrudController<Category, CategoryReadDto, CategoryCreateDto, CategoryUpdateDto>
{
    public CategoryController(AbstractCrudHandler<Category, CategoryReadDto, CategoryCreateDto, CategoryUpdateDto> handler) : base(handler)
    {
    }
}

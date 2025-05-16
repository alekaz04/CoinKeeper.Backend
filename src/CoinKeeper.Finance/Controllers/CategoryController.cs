using CoinKeeper.Common;

namespace CoinKeeper.Finance;

/// <summary>
/// Контроллер для сущности <see cref="Category"/>
/// </summary>
public class CategoryController : AbstractCrudController<Category, CategoryReadDto, CategoryCreateDto>
{
    public CategoryController(AbstractCrudHandler<Category, CategoryReadDto, CategoryCreateDto> handler) : base(handler)
    {
    }
}

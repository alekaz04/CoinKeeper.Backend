using CoinKeeper.Common;

namespace CoinKeeper.Finance;

public class CategoryController : AbstractCrudController<Category, CategoryReadDto, CategoryCreateDto>
{
    public CategoryController(AbstractCrudHandler<Category, CategoryReadDto, CategoryCreateDto> handler) : base(handler)
    {
    }
}

using CoinKeeper.Common;
using Microsoft.AspNetCore.Mvc;

namespace CoinKeeper.Finance;

public class CategoryController : CommonApiController
{
    private readonly CategoryCrudHandler _handler;

    public CategoryController(CategoryCrudHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<Guid> CreateCategory([FromBody] CategoryCreateDto category, CancellationToken token)
    {
        return await _handler.CreateCategory(category, token);
    }

    [HttpGet("{id:guid}")]
    public async Task<CategoryReadDto> GetCategoryById(Guid id, CancellationToken token)
    {
        return await _handler.GetCategoryById(id, token);
    }
}

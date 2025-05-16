namespace CoinKeeper.Finance;

/// <summary>
/// Идентификатор получения категории
/// </summary>
public class CategoryReadDto : ICategory
{
    /// <summary>
    /// Идентификатор категории
    /// </summary>
    public Guid Id { get; set; }

    /// <inheritdoc />
    public string? CategoryName { get; set; }
}

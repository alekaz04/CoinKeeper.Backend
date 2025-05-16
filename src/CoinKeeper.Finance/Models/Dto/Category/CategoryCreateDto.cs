namespace CoinKeeper.Finance;

/// <summary>
/// Дто создания категории
/// </summary>
public class CategoryCreateDto : ICategory
{
    /// <inheritdoc />
    public string CategoryName { get; set; } = string.Empty;
}

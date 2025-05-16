namespace CoinKeeper.Finance;

/// <summary>
/// Дто создания операции
/// </summary>
public class OperationCreateDto : IOperation
{
    /// <inheritdoc />
    public DateTimeOffset OperationTime { get; set; }

    /// <inheritdoc />
    public decimal Amount { get; set; }

    /// <inheritdoc />
    public string Description { get; set; } = string.Empty;

    /// <inheritdoc />
    public OperationType OperationType { get; set; }

    /// <inheritdoc />
    public Guid CategoryId { get; set; }
}

namespace CoinKeeper.Finance;

public class OperationUpdateDto
{
    /// <inheritdoc/>
    public DateTimeOffset OperationTime { get; set; }

    /// <inheritdoc/>
    public decimal Amount { get; set; }

    /// <inheritdoc/>
    public string Description { get; set; } = string.Empty;

    /// <inheritdoc/>
    public OperationType OperationType { get; set; }

    /// <summary>
    /// Идентификатор категории к которой относится операция
    /// </summary>
    public Guid CategoryId { get; set; }
}

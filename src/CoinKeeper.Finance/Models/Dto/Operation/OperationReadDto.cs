namespace CoinKeeper.Finance;

/// <summary>
/// Идентификатор получения операции
/// </summary>
public class OperationReadDto : IOperation
{
    /// <summary>
    /// Идентфикатор
    /// </summary>
    public Guid Id { get; set; }

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

    /// <summary>
    /// Категория операции
    /// </summary>
    public CategoryReadDto? Category { get; set; }
}

namespace CoinKeeper.Finance;

public class OperationReadDto : IOperation
{
    public Guid Id { get; set; }
    public DateTimeOffset OperationTime { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public OperationType OperationType { get; set; }
    public CategoryReadDto Category { get; set; }
}

namespace CoinKeeper.Finance;

public class OperationCreateDto : IOperation
{
    public DateTimeOffset OperationTime { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public OperationType OperationType { get; set; }
    public Guid CategoryId { get; set; }
}

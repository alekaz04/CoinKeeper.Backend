namespace CoinKeeper.Operations;

public class OperationCreateDto : IOperation
{
    public DateTimeOffset OperationTime { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
}

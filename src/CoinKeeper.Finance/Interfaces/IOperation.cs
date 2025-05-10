namespace CoinKeeper.Finance;

public interface IOperation
{
    public DateTimeOffset OperationTime { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public OperationType OperationType { get; set; }
}

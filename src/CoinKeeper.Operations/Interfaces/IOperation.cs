namespace CoinKeeper.Operations;

public interface IOperation
{
    public DateTimeOffset OperationTime { get; set; }
    public decimal Amount { get; set; }
    public bool State { get; set; }
    public string Description { get; set; }
}

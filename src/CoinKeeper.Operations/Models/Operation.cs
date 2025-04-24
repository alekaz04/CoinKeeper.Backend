namespace CoinKeeper.Operations;

public class Operation
{
    public Guid Id { get; set; }
    public DateTimeOffset OperationTime { get; set; }
    public decimal Amount { get; set; }
    public bool State {get; set;}
    public string Description { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

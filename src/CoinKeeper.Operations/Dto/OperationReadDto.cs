namespace CoinKeeper.Operations.Dto;

public class OperationReadDto
{
    public Guid Id { get; set; }
    public DateTimeOffset OperationTime { get; set; }
    public decimal Amount { get; set; }
    public bool State {get; set;}
    public string Description { get; set; } = string.Empty;
}

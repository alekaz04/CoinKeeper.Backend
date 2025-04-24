namespace CoinKeeper.Operations.Dto;

public class OperationCreateDto
{
    public DateTimeOffset OperationTime { get; set; }
    public decimal Amount { get; set; }
    public bool State { get; set; }
    public string Description { get; set; } = string.Empty;
}

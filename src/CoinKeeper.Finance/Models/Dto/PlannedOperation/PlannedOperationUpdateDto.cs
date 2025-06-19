namespace CoinKeeper.Finance;

public class PlannedOperationUpdateDto
{
    public string Name { get; set; } = null!;
    public decimal Amount { get; set; }
    public OperationType OperationType { get; set; }
    public Guid CategoryId { get; set; }
    public Guid AccountId { get; set; }
    public DateTimeOffset NextExecutionDate { get; set; }
    public bool IsActive { get; set; }
}

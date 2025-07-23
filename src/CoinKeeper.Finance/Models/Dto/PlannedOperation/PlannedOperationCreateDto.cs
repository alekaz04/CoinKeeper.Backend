namespace CoinKeeper.Finance;

public class PlannedOperationCreateDto
{
    public string Name { get; set; } = null!;
    public decimal Amount { get; set; }
    public OperationType OperationType { get; set; }
    public string Description { get; set; } = string.Empty;

    public Guid CategoryId { get; set; }
    public Guid AccountId { get; set; }

    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public DateTimeOffset NextExecutionDate { get; set; }

    public int Frequency { get; set; } = 1;
    public FrequencyType FrequencyType { get; set; }
    public TimeSpan ScheduledTime { get; set; }

    public int? MaxExecutions { get; set; }
    public bool IsActive { get; set; } = true;
}

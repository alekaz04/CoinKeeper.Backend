namespace CoinKeeper.Finance;

public interface IPlannedOperation
{
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public OperationType OperationType { get; set; }
    public Guid CategoryId { get; set; }
    public Guid AccountId { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public DateTimeOffset NextExecutionDate { get; set; }
    public int Frequency { get; set; }
    public FrequencyType FrequencyType { get; set; }
    public TimeSpan ScheduledTime { get; set; }
    public int? MaxExecutions { get; set; }
    public int ExecutedCount { get; set; }
    public bool IsPaused { get; set; }
    public bool IsActive { get; set; }
}

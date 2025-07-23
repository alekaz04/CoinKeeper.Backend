using CoinKeeper.Authentication.Domain;
using CoinKeeper.Common.Domain;

namespace CoinKeeper.Finance;

public class PlannedOperation : IBaseEntity, IUserSpecifiedEntity, IPlannedOperation
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public string Name { get; set; } = null!;
    public decimal Amount { get; set; }

    public string Description { get; set; } = string.Empty;

    public OperationType OperationType { get; set; }
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    public Guid AccountId { get; set; }
    public Account? Account { get; set; }

    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public DateTimeOffset NextExecutionDate { get; set; }

    public int Frequency { get; set; } = 1; // Каждые N периодов
    public FrequencyType FrequencyType { get; set; }
    public TimeSpan ScheduledTime { get; set; }

    public int? MaxExecutions { get; set; } // Максимальное количество выполнений
    public int ExecutedCount { get; set; }  // Количество выполненных операций
    public bool IsPaused { get; set; }  // Приостановлена ли операция

    public bool IsActive { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
}

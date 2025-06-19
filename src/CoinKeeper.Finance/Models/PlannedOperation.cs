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
    public OperationType OperationType { get; set; }
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    public Guid AccountId { get; set; }
    public Account? Account { get; set; }
    public DateTimeOffset NextExecutionDate { get; set; }
    public bool IsActive { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
}

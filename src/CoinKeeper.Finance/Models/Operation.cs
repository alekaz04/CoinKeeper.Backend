using CoinKeeper.Authentication.Domain;
using CoinKeeper.Common.Domain;

namespace CoinKeeper.Finance;

public class Operation : IBaseEntity, IUserSpecifiedEntity, IOperation
{
    public Guid Id { get; set; }
    public DateTimeOffset OperationTime { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public OperationType OperationType { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }
}

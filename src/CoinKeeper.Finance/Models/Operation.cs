using CoinKeeper.Authentication;
using CoinKeeper.Common;

namespace CoinKeeper.Finance;

public class Operation : IBaseEntity, IOperation
{
    public Guid Id { get; set; }
    public DateTimeOffset OperationTime { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }

    public Guid UserId { get; set; }

    public User? User { get; set; }
}

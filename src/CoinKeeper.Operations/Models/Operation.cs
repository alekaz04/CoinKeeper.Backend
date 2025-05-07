using CoinKeeper.Common;

namespace CoinKeeper.Operations;

public class Operation : IBaseEntity
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
}

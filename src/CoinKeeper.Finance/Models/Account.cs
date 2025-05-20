using CoinKeeper.Common.Domain;

namespace CoinKeeper.Finance;

public class Account : IBaseEntity
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}

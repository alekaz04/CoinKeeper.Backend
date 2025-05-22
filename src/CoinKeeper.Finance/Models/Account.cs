using CoinKeeper.Authentication.Domain;
using CoinKeeper.Common.Domain;

namespace CoinKeeper.Finance;

public class Account : IBaseEntity, IAccount, IUserSpecifiedEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public decimal Balance { get; set; }
    public AccountType Type { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
}

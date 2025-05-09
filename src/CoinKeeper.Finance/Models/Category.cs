using CoinKeeper.Authentication.Domain;
using CoinKeeper.Common.Domain;

namespace CoinKeeper.Finance;

public class Category : IBaseEntity, IUserSpecifiedEntity, ICategory
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<Operation> Operations { get; set; }

    public Guid UserId { get; set; }

    public User? User { get; set; }
}

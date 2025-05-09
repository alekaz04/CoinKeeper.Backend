using CoinKeeper.Authentication;
using CoinKeeper.Common;

namespace CoinKeeper.Finance;

public class Category : IBaseEntity, ICategory
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

using CoinKeeper.Common;

namespace CoinKeeper.Authentication;

public class User : IBaseEntity
{
    public Guid Id { get; set; }
    public string Login { get; set; } = null!;
    public string? PasswordHash { get; set; }
    public string? PasswordSalt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTimeOffset LastActivityTime { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}

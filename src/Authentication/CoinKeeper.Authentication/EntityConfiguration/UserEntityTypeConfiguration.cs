using CoinKeeper.Authentication.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoinKeeper.Authentication;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasData(new List<User>()
        {
            new()
            {
                Id = new("00000000-0000-0000-0000-000000000001"),
                Login = "dev",
                PasswordHash = "TGnvarYGHoF/sKZ+pRblGK4BojpIqFjzpnU+3nLDhdc=",
                PasswordSalt = "HPdneFiNa8P3C92uUdWLGA==",
                LastActivityTime = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
                CreatedAt = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
                UpdatedAt = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
                IsDeleted = false
            }
        });
    }
}

namespace CoinKeeper.Authentication.Domain;

public class JwtOptions
{
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string SecurityKey { get; set; } = null!;
    public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(15);
}

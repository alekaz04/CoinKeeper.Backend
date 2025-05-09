namespace CoinKeeper.Authentication;

public class AuthToken
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public TimeSpan Expires { get; set; }
}

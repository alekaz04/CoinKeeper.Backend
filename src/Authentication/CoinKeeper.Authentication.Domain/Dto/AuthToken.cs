namespace CoinKeeper.Authentication.Domain;

/// <summary>
/// Пара Access и Refresh токен
/// </summary>
public class AuthToken
{
    /// <summary>
    /// Access токен
    /// </summary>
    public string AccessToken { get; set; } = null!;

    /// <summary>
    /// Refresh токен
    /// </summary>
    public string RefreshToken { get; set; } = null!;

    /// <summary>
    /// Время жизни токена
    /// </summary>
    public TimeSpan Expires { get; set; }
}

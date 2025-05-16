namespace CoinKeeper.Authentication.Domain;

/// <summary>
/// Опции JWT аутентификации
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// Издатель
    /// </summary>
    public string Issuer { get; set; } = null!;

    /// <summary>
    /// Пользователь
    /// </summary>
    public string Audience { get; set; } = null!;

    /// <summary>
    /// Ключ
    /// </summary>
    public string SecurityKey { get; set; } = null!;

    /// <summary>
    /// Время жизни Access токена
    /// </summary>
    /// <remarks>По умолчанию 15 минут</remarks>
    public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(15);
}

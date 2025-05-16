namespace CoinKeeper.Authentication.Domain;

/// <summary>
/// Дто пароль пользователя
/// </summary>
public class UserPasswordDto
{
    /// <summary>
    /// Хэш пароля
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    /// Соль
    /// </summary>
    public string Salt { get; set; } = null!;
}

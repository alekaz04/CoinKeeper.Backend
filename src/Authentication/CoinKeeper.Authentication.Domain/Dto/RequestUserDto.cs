namespace CoinKeeper.Authentication.Domain;

/// <summary>
/// Модель для авторизации пользователя
/// </summary>
public class RequestUserDto
{
    /// <summary>
    /// Лоигин
    /// </summary>
    public string Login { get; set; } = null!;

    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; } = null!;
}

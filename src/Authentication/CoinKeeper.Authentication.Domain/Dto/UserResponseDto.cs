namespace CoinKeeper.Authentication.Domain;

/// <summary>
/// Дто для возвращении при авторизации пользователя
/// </summary>
public class UserResponseDto
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Лоигн
    /// </summary>
    public string Login { get; set; } = null!;

    /// <inheritdoc cref="AuthToken"/>
    public AuthToken? Token { get; set; }
}

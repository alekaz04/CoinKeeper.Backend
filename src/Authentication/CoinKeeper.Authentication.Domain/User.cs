using CoinKeeper.Common.Domain;

namespace CoinKeeper.Authentication.Domain;

/// <summary>
/// Пользователь
/// </summary>
public class User : IBaseEntity
{
    /// <inheritdoc />
    public Guid Id { get; set; }

    /// <summary>
    /// Логин
    /// </summary>
    public string Login { get; set; } = null!;

    /// <summary>
    /// Хэш пароля
    /// </summary>
    public string? PasswordHash { get; set; }

    /// <summary>
    /// Соль пароля
    /// </summary>
    public string? PasswordSalt { get; set; }

    /// <summary>
    /// Refresh токен
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <inheritdoc />
    public DateTimeOffset CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTimeOffset UpdatedAt { get; set; }

    /// <inheritdoc />
    public bool IsDeleted { get; set; }
}

namespace CoinKeeper.Authentication.Domain;

/// <summary>
/// Интерфейс для сущности у которого есть идентфикатор пользователя
/// </summary>
public interface IUserSpecifiedEntity
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Пользователь владелец
    /// </summary>
    public User? User { get; set; }
}

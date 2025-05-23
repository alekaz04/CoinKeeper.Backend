namespace CoinKeeper.Authentication.Domain;

/// <summary>
/// Сервис для получения текущего пользователя
/// </summary>
public interface ICurrentUser
{
    /// <summary>
    /// Получить идентификатор текущего пользователя
    /// </summary>
    public Guid GetCurrentUserId();

    /// <summary>
    /// Попробовать получить идентификатор текущего пользователя
    /// </summary>
    /// <returns>Идентфикатор пользавателя, если нет то null</returns>
    public Guid? TryGetCurrentUserId();
}

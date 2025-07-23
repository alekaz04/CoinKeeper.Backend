namespace CoinKeeper.Common.Domain;

/// <summary>
/// Базовая сущность системы
/// </summary>
public interface IBaseEntity
{
    /// <summary>
    /// Идентификатор объекта
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Время создания объекта
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Время последнего обновления объекта
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// Флаг удаления
    /// </summary>
    public bool IsDeleted { get; set; }
}

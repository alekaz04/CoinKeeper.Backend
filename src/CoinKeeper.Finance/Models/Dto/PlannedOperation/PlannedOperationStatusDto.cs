namespace CoinKeeper.Finance;

/// <summary>
/// DTO для ответа операций изменения статуса планового платежа
/// </summary>
public class PlannedOperationStatusDto
{
    /// <summary>
    /// Идентификатор планового платежа
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Название планового платежа
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Активен ли плановый платеж
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Приостановлен ли плановый платеж
    /// </summary>
    public bool IsPaused { get; set; }

    /// <summary>
    /// Дата следующего выполнения
    /// </summary>
    public DateTimeOffset NextExecutionDate { get; set; }

    /// <summary>
    /// Количество выполненных операций
    /// </summary>
    public int ExecutedCount { get; set; }

    /// <summary>
    /// Максимальное количество выполнений (если ограничено)
    /// </summary>
    public int? MaxExecutions { get; set; }

    /// <summary>
    /// Сообщение о результате операции
    /// </summary>
    public string Message { get; set; } = string.Empty;
}

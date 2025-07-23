namespace CoinKeeper.Finance;

/// <summary>
/// DTO для списка предварительного просмотра
/// </summary>
public class PlannedOperationPreviewListDto
{
    /// <summary>
    /// Список будущих выполнений
    /// </summary>
    public List<PlannedOperationPreviewDto> Executions { get; set; } = [];

    /// <summary>
    /// Общее количество запланированных выполнений (если ограничено)
    /// </summary>
    public int? TotalExecutions { get; set; }

    /// <summary>
    /// Количество уже выполненных операций
    /// </summary>
    public int ExecutedCount { get; set; }

    /// <summary>
    /// Статус планового платежа
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Приостановлен ли плановый платеж
    /// </summary>
    public bool IsPaused { get; set; }
}

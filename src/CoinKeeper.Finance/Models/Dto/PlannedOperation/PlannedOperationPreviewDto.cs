namespace CoinKeeper.Finance;

/// <summary>
/// DTO для предварительного просмотра будущих выполнений планового платежа
/// </summary>
public class PlannedOperationPreviewDto
{
    /// <summary>
    /// Дата и время выполнения
    /// </summary>
    public DateTimeOffset ExecutionDate { get; set; }

    /// <summary>
    /// Сумма операции
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Описание операции
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Номер выполнения (порядковый номер в последовательности)
    /// </summary>
    public int ExecutionNumber { get; set; }
}

/// <summary>
/// DTO для списка предварительного просмотра
/// </summary>
public class PlannedOperationPreviewListDto
{
    /// <summary>
    /// Список будущих выполнений
    /// </summary>
    public List<PlannedOperationPreviewDto> Executions { get; set; } = new();

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

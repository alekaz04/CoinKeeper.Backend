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

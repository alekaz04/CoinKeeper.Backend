namespace CoinKeeper.Finance;

/// <summary>
/// DTO для ручного выполнения планового платежа
/// </summary>
public class PlannedOperationExecuteDto
{
    /// <summary>
    /// Дата и время выполнения операции (если не указано, используется текущее время)
    /// </summary>
    public DateTimeOffset? ExecutionDate { get; set; }

    /// <summary>
    /// Переопределить сумму для данного выполнения (если не указано, используется сумма из планового платежа)
    /// </summary>
    public decimal? OverrideAmount { get; set; }

    /// <summary>
    /// Дополнительное описание для данного выполнения
    /// </summary>
    public string? AdditionalDescription { get; set; }
}

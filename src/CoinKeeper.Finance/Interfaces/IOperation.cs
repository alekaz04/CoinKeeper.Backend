namespace CoinKeeper.Finance;

/// <summary>
/// Базовй интерфейс операции
/// </summary>
public interface IOperation
{
    /// <summary>
    /// Дата и время
    /// </summary>
    public DateTimeOffset OperationTime { get; set; }

    /// <summary>
    /// Сумма
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Тип
    /// </summary>
    public OperationType OperationType { get; set; }

    /// <summary>
    /// Идентфикатор категарии траты
    /// </summary>
    public Guid CategoryId { get; set; }
}

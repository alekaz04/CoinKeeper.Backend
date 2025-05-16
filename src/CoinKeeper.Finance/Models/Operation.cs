using CoinKeeper.Authentication.Domain;
using CoinKeeper.Common.Domain;

namespace CoinKeeper.Finance;

/// <summary>
/// Сущность опреации
/// </summary>
public class Operation : IBaseEntity, IUserSpecifiedEntity, IOperation
{
    /// <inheritdoc/>
    public Guid Id { get; set; }

    /// <inheritdoc/>
    public DateTimeOffset OperationTime { get; set; }

    /// <inheritdoc/>
    public decimal Amount { get; set; }

    /// <inheritdoc/>
    public string Description { get; set; } = string.Empty;

    /// <inheritdoc/>
    public OperationType OperationType { get; set; }

    /// <inheritdoc/>
    public DateTimeOffset CreatedAt { get; set; }

    /// <inheritdoc/>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <inheritdoc/>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Идентификатор категории к которой относится операция
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Сущность категории
    /// </summary>
    public Category? Category { get; set; }

    /// <inheritdoc/>
    public Guid UserId { get; set; }

    /// <inheritdoc/>
    public User? User { get; set; }
}

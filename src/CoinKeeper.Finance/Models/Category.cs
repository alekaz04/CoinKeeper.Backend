using CoinKeeper.Authentication.Domain;
using CoinKeeper.Common.Domain;

namespace CoinKeeper.Finance;

/// <summary>
/// Сущность категория
/// </summary>
public class Category : IBaseEntity, IUserSpecifiedEntity, ICategory
{
    /// <inheritdoc />
    public Guid Id { get; set; }

    /// <inheritdoc />
    public string CategoryName { get; set; } = null!;

    /// <inheritdoc />
    public DateTimeOffset CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTimeOffset UpdatedAt { get; set; }

    /// <inheritdoc />
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Операции этой категории
    /// </summary>
    public ICollection<Operation> Operations { get; set; } = [];

    /// <inheritdoc />
    public Guid UserId { get; set; }

    /// <inheritdoc />
    public User? User { get; set; }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoinKeeper.Finance;

/// <summary>
/// Конфигурация сущности <see cref="Operation"/> для ef core
/// </summary>
public class OperationTypeConfiguration : IEntityTypeConfiguration<Operation>
{
    public void Configure(EntityTypeBuilder<Operation> builder)
    {
        builder.HasKey(x => x.Id);
    }
}

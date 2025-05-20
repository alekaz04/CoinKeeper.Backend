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

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Operations)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.Amount).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Description).HasMaxLength(500);
    }
}

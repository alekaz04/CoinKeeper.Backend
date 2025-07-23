using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoinKeeper.Finance;

/// <summary>
/// Конфигурация сущности <see cref="PlannedOperation"/> для ef core
/// </summary>
public class PlannedOperationEntityTypeConfiguration : IEntityTypeConfiguration<PlannedOperation>
{
    public void Configure(EntityTypeBuilder<PlannedOperation> builder)
    {
        builder.ToTable("PlannedOperations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder.Property(x => x.OperationType)
            .HasConversion<int>();

        builder.Property(x => x.FrequencyType)
            .HasConversion<int>();

        builder.Property(x => x.Frequency)
            .HasDefaultValue(1);

        builder.Property(x => x.ExecutedCount)
            .HasDefaultValue(0);

        builder.Property(x => x.IsPaused)
            .HasDefaultValue(false);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        // Relationships
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Account)
            .WithMany()
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.NextExecutionDate);
        builder.HasIndex(x => new { x.IsActive, x.IsDeleted, x.IsPaused });
        builder.HasIndex(x => x.StartDate);
        builder.HasIndex(x => x.EndDate);
    }
}

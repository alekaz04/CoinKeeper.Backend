using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoinKeeper.Finance;

/// <summary>
/// Конфигурация сущности <see cref="Category"/> для ef core
/// </summary>
public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Operations)
            .WithOne(x => x.Category)
            .HasForeignKey(x => x.CategoryId)
            .HasPrincipalKey(x => x.Id);
    }
}

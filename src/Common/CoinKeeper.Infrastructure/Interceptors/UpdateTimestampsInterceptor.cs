using CoinKeeper.Common.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CoinKeeper.Infrastructure;

public class UpdateTimestampsInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateTimestamps(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateTimestamps(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateTimestamps(DbContext? context)
    {
        if (context == null)
        {
            return;
        }

        var now = DateTimeOffset.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries<IBaseEntity>()
                     .Where(e => e.State == EntityState.Added))
        {
            entry.Property(e => e.CreatedAt).CurrentValue = now;
            entry.Property(e => e.UpdatedAt).CurrentValue = now;
        }

        foreach (var entry in context.ChangeTracker.Entries<IBaseEntity>()
                     .Where(e => e.State == EntityState.Modified))
        {
            entry.Property(e => e.UpdatedAt).CurrentValue = now;

            if (entry.Property(e => e.CreatedAt).IsModified)
            {
                entry.Property(e => e.CreatedAt).IsModified = false;
            }
        }
    }

}

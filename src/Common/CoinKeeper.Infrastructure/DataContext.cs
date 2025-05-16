using Microsoft.EntityFrameworkCore;

namespace CoinKeeper.Infrastructure;

/// <summary>
/// Контекст доступа к базе данных
/// </summary>
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        var configurationTypes = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName.StartsWith("CoinKeeper"))
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract)
            .SelectMany(t => t.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
            .Where(ti => ti.Interface.IsGenericType &&
                         ti.Interface.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
            .Select(ti => ti.Type);

        foreach (var type in configurationTypes)
        {
            dynamic configurationInstance = Activator.CreateInstance(type);
            modelBuilder.ApplyConfiguration(configurationInstance);
        }
    }
}

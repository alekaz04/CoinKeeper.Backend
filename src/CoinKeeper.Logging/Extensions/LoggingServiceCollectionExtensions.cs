using CoinKeeper.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.Configuration;
using NpgsqlTypes;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using Serilog.Sinks.PostgreSQL.ColumnWriters;

namespace CoinKeeper.Extensions.DependencyInjection;

/// <summary>
/// Класс расширений <see cref="IServiceCollection"/> для модуля логирования
/// </summary>
public static class LoggingServiceCollectionExtensions
{
    /// <summary>
    /// Добавить логгирование
    /// </summary>
    public static IServiceCollection AddCoinKeeperLogging(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Logs") ?? throw new InvalidConfigurationException("Connection string not found");

        IDictionary<string, ColumnWriterBase> columnWriters = new Dictionary<string, ColumnWriterBase>
        {
            {nameof(LogEntity.Id), new GuidColumnWriter(NpgsqlDbType.Uuid) },
            {nameof(LogEntity.Level), new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
            {nameof(LogEntity.RaiseDate), new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
            {nameof(LogEntity.Message), new RenderedMessageColumnWriter(NpgsqlDbType.Text)  },
            {nameof(LogEntity.Exception), new ExceptionColumnWriter(NpgsqlDbType.Text) },
            {nameof(LogEntity.UserId), new SinglePropertyColumnWriter(nameof(LogEntity.UserId), PropertyWriteMethod.ToString, NpgsqlDbType.Text, "l") },
            {nameof(LogEntity.LogEventProperties), new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) }
        };

        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .WriteTo.PostgreSQL(connectionString,
                "Logs",
                columnWriters,
                schemaName: "public",
                useCopy: false,
                period: TimeSpan.FromSeconds(5),
                needAutoCreateTable: true,
                failureCallback: ex => Console.WriteLine($"Sink error: {ex.Message}"))
            .WriteTo.Console()
            .Enrich.FromLogContext()
            .Enrich.With<UserIdEnricher>()
            .CreateLogger();

        services.AddLogging()
            .AddSerilog(logger);

        services.AddScoped<UserIdEnricher>();

        return services;
    }
}

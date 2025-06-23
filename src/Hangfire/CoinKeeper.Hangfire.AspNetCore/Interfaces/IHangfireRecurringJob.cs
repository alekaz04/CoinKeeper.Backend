using Hangfire;

namespace CoinKeeper.Hangfire.AspNetCore;

/// <summary>
/// Интерфейс для создания повторяющихся задач Hangfire
/// </summary>
public interface IHangfireRecurringJob
{
    /// <summary>
    /// Периодичность запуска выполнения заданий в Cron - формате
    /// </summary>
    string CronExpression { get; }

    /// <summary>
    /// Параметры запуска заданий
    /// </summary>
    /// <remarks>Если не заданно, то по умолчанию используется настройка: <br/>
    /// <b>new RecurringJobOptions { TimeZone = TimeZoneInfo.Local }</b>
    /// </remarks>
    RecurringJobOptions? JobOptions { get; }

    /// <summary>
    /// Реализация логики выполнения задачи
    /// </summary>
    /// <param name="token">Токен отмены</param>
    Task Execute(CancellationToken token);

    /// <summary>
    /// Идентификатор задачи
    /// </summary>
    /// <remarks>Если не реализовывать, то в качестве идентификатора будет использоваться имя типа</remarks>
    string? JobId()
    {
        return null;
    }
}

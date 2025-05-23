using CoinKeeper.Authentication;
using CoinKeeper.Authentication.Domain;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace CoinKeeper.Logging;

/// <summary>
/// Энричер идентфикатора пользователя
/// </summary>
public class UserIdEnricher : ILogEventEnricher
{
    /// <inheritdoc cref="ICurrentUser"/>
    private readonly ICurrentUser _currentUser;

    public UserIdEnricher()
    {
        _currentUser = new CurrentUserService(new HttpContextAccessor());
    }

    public UserIdEnricher(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    /// <inheritdoc />
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var userId = _currentUser.TryGetCurrentUserId();

        if (userId.HasValue)
        {
            var property = propertyFactory.CreateProperty(nameof(LogEntity.UserId), userId.Value.ToString());
            logEvent.AddPropertyIfAbsent(property);
        }
        else
        {
            var property = propertyFactory.CreateProperty(nameof(LogEntity.UserId), null);
            logEvent.AddPropertyIfAbsent(property);
        }
    }
}

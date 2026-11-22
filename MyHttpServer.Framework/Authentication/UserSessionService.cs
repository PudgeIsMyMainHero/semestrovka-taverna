using System.Collections.Concurrent;

namespace MyHttpServer.Framework.Authentication;

public sealed class UserSessionService : IUserSessionService
{
    private readonly ConcurrentDictionary<string, SessionInfo>
        _sessions = new();

    private static readonly TimeSpan SessionLifetime =
        TimeSpan.FromMinutes(30);

    /// <inheritdoc />
    public string CreateSession(int userId, bool isAdmin)
    {
        string sessionId = Guid.NewGuid().ToString("N");

        var session = new SessionInfo(
            userId,
            isAdmin,
            DateTimeOffset.UtcNow.Add(SessionLifetime));

        if (!_sessions.TryAdd(sessionId, session))
        {
            throw new InvalidOperationException(
                "Не удалось создать уникальный сеанс.");
        }

        return sessionId;
    }

    /// <inheritdoc />
    public int? GetUserId(string? sessionId)
    {
        return GetActiveSession(sessionId)?.UserId;
    }

    /// <inheritdoc />
    public bool? GetIsAdmin(string? sessionId)
    {
        return GetActiveSession(sessionId)?.IsAdmin;
    }

    /// <inheritdoc />
    public void RemoveSession(string? sessionId)
    {
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            return;
        }

        _sessions.TryRemove(sessionId, out _);
    }


    private SessionInfo? GetActiveSession(string? sessionId)
    {
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            return null;
        }

        if (!_sessions.TryGetValue(
                sessionId,
                out SessionInfo? session))
        {
            return null;
        }

        if (session.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            _sessions.TryRemove(sessionId, out _);

            return null;
        }

        return session;
    }

    private sealed record SessionInfo(
        int UserId,
        bool IsAdmin,
        DateTimeOffset ExpiresAt);
}

using Microsoft.AspNetCore.SignalR;

namespace IMessenger.Server.Authorization;

public class CustomUserIdProvider : IUserIdProvider
{
    public virtual string? GetUserId(HubConnectionContext connection)
    {
        return connection?.User?.Identity?.Name;
    }
}

using IMessenger.Server.Model;
using IMessenger.Server.Src;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace IMessenger.Server.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();
    private ApplicationContext _context;

    public ChatHub(ApplicationContext applicationContext)
    {
        _context = applicationContext;
    }

    public async Task SendChatMessage(Message message)
    {
        try
        {
            if (_context?.Users.FirstOrDefault(p => p.Userid.Equals(message.Receiverid)) is null)
                throw new Exception($"user \"{message.Receiverid}\" does not exist");

            var connections = _connections.GetConnections(message.Receiverid);

            if (connections is not null)
            {
                foreach (var connectionId in connections)
                {
                    try
                    {
                        Clients.Client(connectionId)?.SendAsync("ReceiveMessage", message);
                        await _context.Messages.AddAsync(message);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex?.InnerException?.Message);
                    }
                }
            }
            else
            {
                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public override Task OnConnectedAsync()
    {
        _connections?.Add(Context?.User?.Identity?.Name, Context?.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _connections?.Remove(Context?.User?.Identity?.Name, Context?.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}

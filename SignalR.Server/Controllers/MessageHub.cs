using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SignalR.Server.Controllers;

[Authorize]
public class MessageHub : Hub
{
    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("ReceivedMessage", Context.UserIdentifier, message);
    }
}
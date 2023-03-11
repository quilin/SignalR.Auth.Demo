using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace SignalR.Server.Controllers;

public class MessageHub : Hub
{
    public async Task SendMessage(string message)
    {
        var claimsPrincipal = Context.User;
        if (claimsPrincipal is null) return;

        var name = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        await Clients.All.SendAsync("ReceivedMessage", name, message);
    }
}
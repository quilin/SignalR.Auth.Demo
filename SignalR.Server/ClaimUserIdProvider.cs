using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace SignalR.Server;

public class ClaimUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection) => 
        connection.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
}
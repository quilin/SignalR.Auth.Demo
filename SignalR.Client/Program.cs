// See https://aka.ms/new-console-template for more information

using Microsoft.AspNetCore.SignalR.Client;
using SignalR.Server;

HubConnection? connection = null;
while (true)
{
    var command = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(command)) break;

    if (command.StartsWith("I am "))
    {
        var login = command["I am ".Length..];
        var (success, newConnection) = await ServerStuff.EstablishConnection(login);
        if (!success)
        {
            Console.WriteLine("Unable to log you in");
            continue;
        }

        if (connection is not null)
            await connection.DisposeAsync();
        connection = newConnection!;
        await connection.StartAsync();
    }
    else if (command.StartsWith("Send ") && connection is not null)
    {
        try
        {
            await connection.SendAsync("SendMessage", command["Send ".Length..]);
        }
        catch (Exception)
        {
            Console.WriteLine("Unable to send message...");
        }
    }
    else if (command == "Logout" && connection is not null)
    {
        await connection.DisposeAsync();
        connection = null;
    }
    else if (command == "Upload")
    {
        await ServerStuff.GenerateAndSendFile();
    }
}

Console.WriteLine("Exit");

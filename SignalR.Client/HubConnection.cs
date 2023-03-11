using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalR.Server;

public static class ServerStuff
{
    public static async Task<(bool success, HubConnection? connection)> EstablishConnection(string login)
    {
        using var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
        var requestContent = new StringContent(
            JsonSerializer.Serialize(new { login }),
            Encoding.UTF8, MediaTypeNames.Application.Json);
        var responseMessage = await httpClient.PostAsync("/auth", requestContent);

        if (!responseMessage.IsSuccessStatusCode)
            return (false, null);

        var responseString = await responseMessage.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<TokenResponse>(responseString)!.Value.Token;
        Console.WriteLine($"Logged you in as {login} with jwt {token}");

        var hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/messenger", opts =>
                opts.AccessTokenProvider = () => Task.FromResult(token)!)
            .WithAutomaticReconnect()
            .Build();
        hubConnection.On<string, string>("ReceivedMessage",
            (author, message) => Console.WriteLine($"Received message from {author}: {message}"));
        return (true, hubConnection);
    }
    
    public class TokenResponse
    {
        [JsonPropertyName("value")]
        public TokenResponseValue Value { get; set; }

        public class TokenResponseValue
        {
            [JsonPropertyName("token")]
            public string Token { get; set; }
        }
    }
}
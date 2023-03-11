using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace SignalR.Server.Controllers;

public class LoginController : Controller
{
    private const string Key = "long_and_amazing_secret_secure_as_hell";

    private static SigningCredentials SigningCredentials => new(
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key)),
        SecurityAlgorithms.HmacSha256Signature);

    public class LoginCredentials
    {
        public string Login { get; set; }
    }

    [HttpPost("auth")]
    public IResult Auth(
        [FromBody] LoginCredentials credentials)
    {
        if (string.IsNullOrWhiteSpace(credentials.Login))
            return Results.Unauthorized();

        var handler = new JwtSecurityTokenHandler();
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, credentials.Login),
                new Claim(ClaimTypes.AuthenticationMethod, "custom")
            }),
            SigningCredentials = SigningCredentials,
            Expires = DateTime.UtcNow + TimeSpan.FromHours(10),
        };

        var token = handler.CreateToken(descriptor);
        return Results.Json(new { token = handler.WriteToken(token) });
    }

    [HttpGet("test")]
    [Authorize]
    public IResult Test() => Results.Json(new { claims = User.Claims.ToDictionary(c => c.Type, c => c.Value) });
}
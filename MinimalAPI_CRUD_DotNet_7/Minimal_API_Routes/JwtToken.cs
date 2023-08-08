using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace test.Minimal_API_Routes
{
    public class Login
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
    public static class JwtToken
    {
        public static void CreateJwtToken(this WebApplication app)
        {
            var builder = app;
            app.MapPost("/security/createToken", [AllowAnonymous] (Login login) =>
            {
                if (login != null)
                {
                    if (login.Username == "deepakfartiyal@softprodigy.com" && login.Password == "deepak@1982")
                    {
                        var issuer = builder.Configuration["Jwt:Issuer"];
                        var audience = builder.Configuration["Jwt:Audience"];
                        var key = Encoding.ASCII.GetBytes
                        (builder.Configuration["Jwt:Key"]);
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new[]
                            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, login.Username),
                new Claim(JwtRegisteredClaimNames.Email,login.Password),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
                            Expires = DateTime.UtcNow.AddMinutes(5),
                            Issuer = issuer,
                            Audience = audience,
                            SigningCredentials = new SigningCredentials
                            (new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha512Signature)
                        };
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        var jwtToken = tokenHandler.WriteToken(token);
                        var stringToken = "Token:-  " + tokenHandler.WriteToken(token);
                        return Results.Ok(stringToken);
                    }
                }
                return Results.Unauthorized();
            });

        }
    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ConstructionPortal.Api.Models;
using Microsoft.IdentityModel.Tokens;

namespace ConstructionPortal.Api.Services;

public class JwtService(IConfiguration config)
{
    public string CreateToken(string username, UserRole role)
    {
        var issuer = config["Jwt:Issuer"]!;
        var audience = config["Jwt:Audience"]!;
        var key = config["Jwt:Key"]!;
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, username),
            new(ClaimTypes.Role, role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

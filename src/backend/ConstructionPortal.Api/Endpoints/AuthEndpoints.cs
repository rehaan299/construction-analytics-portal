using System.Security.Claims;
using ConstructionPortal.Api.Dtos;
using ConstructionPortal.Api.Models;
using ConstructionPortal.Api.Services;

namespace ConstructionPortal.Api.Endpoints;

public static class AuthEndpoints
{
    // Prototype users (in real enterprise: Azure AD / SSO)
    private static readonly Dictionary<string, (string Hash, UserRole Role)> Users =
        new()
        {
            ["field1"] = (PasswordHasher.Hash("Password123!"), UserRole.FieldUser),
            ["pm1"] = (PasswordHasher.Hash("Password123!"), UserRole.ProjectManager),
            ["finance1"] = (PasswordHasher.Hash("Password123!"), UserRole.Finance),
            ["exec1"] = (PasswordHasher.Hash("Password123!"), UserRole.Executive),
            ["admin"] = (PasswordHasher.Hash("Password123!"), UserRole.Admin),
        };

    public static void MapAuth(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", (LoginRequest req, JwtService jwt) =>
        {
            if (!Users.TryGetValue(req.Username, out var u))
                return Results.Unauthorized();

            if (PasswordHasher.Hash(req.Password) != u.Hash)
                return Results.Unauthorized();

            var token = jwt.CreateToken(req.Username, u.Role);
            return Results.Ok(new LoginResponse(token, u.Role.ToString()));
        });

        app.MapGet("/api/auth/me", (ClaimsPrincipal user) =>
        {
            var username = user.Identity?.Name ?? "unknown";
            var role = user.FindFirstValue(ClaimTypes.Role) ?? "unknown";
            return Results.Ok(new MeResponse(username, role));
        }).RequireAuthorization();
    }
}

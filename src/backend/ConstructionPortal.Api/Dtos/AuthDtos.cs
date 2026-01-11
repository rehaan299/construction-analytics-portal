namespace ConstructionPortal.Api.Dtos;

public record LoginRequest(string Username, string Password);
public record LoginResponse(string Token, string Role);
public record MeResponse(string Username, string Role);

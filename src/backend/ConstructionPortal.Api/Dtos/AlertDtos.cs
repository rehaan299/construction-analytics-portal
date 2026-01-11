namespace ConstructionPortal.Api.Dtos;

public record AlertResponse(
    int Id,
    int ProjectId,
    string AlertType,
    string Severity,
    string Message,
    bool Resolved,
    string CreatedAt
);

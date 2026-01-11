namespace ConstructionPortal.Api.Dtos;

public record CostEntryResponse(
    int Id,
    int ProjectId,
    string CostDate,
    string CostCode,
    decimal Amount,
    string? Description,
    string Source,
    string CreatedAt
);

public record ImportCostRequest(
    int ProjectId,
    string CostDate,
    string CostCode,
    decimal Amount,
    string? Description,
    string Source
);

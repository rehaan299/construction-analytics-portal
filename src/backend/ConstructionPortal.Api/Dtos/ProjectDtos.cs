namespace ConstructionPortal.Api.Dtos;

public record ProjectResponse(int Id, string Code, string Name, string StartDate, string EndDatePlanned, decimal Budget);

public record ProjectKpisResponse(
    int ProjectId,
    decimal Budget,
    decimal ActualCostToDate,
    decimal BudgetUsedPercent,
    int LatestProgressPercent,
    int Last7DaysLaborHours,
    decimal Last7DaysCost,
    int ActiveAlertsCount
);

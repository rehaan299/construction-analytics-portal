namespace ConstructionPortal.Api.Dtos;

public record CreateDailyReportRequest(
    int ProjectId,
    string ReportDate,
    int LaborHours,
    int EquipmentHours,
    int ProgressPercent,
    string? Notes
);

public record DailyReportResponse(
    int Id,
    int ProjectId,
    string ReportDate,
    int LaborHours,
    int EquipmentHours,
    int ProgressPercent,
    string? Notes,
    string SubmittedBy,
    string CreatedAt
);

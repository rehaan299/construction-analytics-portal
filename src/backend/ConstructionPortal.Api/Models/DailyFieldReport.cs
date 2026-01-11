namespace ConstructionPortal.Api.Models;

public class DailyFieldReport
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Project? Project { get; set; }

    public DateOnly ReportDate { get; set; }
    public int LaborHours { get; set; }
    public int EquipmentHours { get; set; }
    public int ProgressPercent { get; set; }
    public string? Notes { get; set; }
    public string SubmittedBy { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

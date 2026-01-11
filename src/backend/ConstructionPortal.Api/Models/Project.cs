namespace ConstructionPortal.Api.Models;

public class Project
{
    public int Id { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public DateOnly StartDate { get; set; }
    public DateOnly EndDatePlanned { get; set; }
    public decimal Budget { get; set; }

    public List<DailyFieldReport> DailyFieldReports { get; set; } = new();
    public List<CostEntry> CostEntries { get; set; } = new();
    public List<Alert> Alerts { get; set; } = new();
}

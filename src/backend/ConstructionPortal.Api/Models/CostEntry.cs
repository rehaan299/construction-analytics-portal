namespace ConstructionPortal.Api.Models;

public class CostEntry
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Project? Project { get; set; }

    public DateOnly CostDate { get; set; }
    public string CostCode { get; set; } = "";
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public string Source { get; set; } = "ERP_SIM";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

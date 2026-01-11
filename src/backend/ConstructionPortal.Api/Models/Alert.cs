namespace ConstructionPortal.Api.Models;

public class Alert
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Project? Project { get; set; }

    public string AlertType { get; set; } = "";
    public string Severity { get; set; } = "Info"; // Info/Warning/Critical
    public string Message { get; set; } = "";
    public bool Resolved { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

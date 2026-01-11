using ConstructionPortal.Api.Data;
using ConstructionPortal.Api.Dtos;
using ConstructionPortal.Api.Models;
using ConstructionPortal.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace ConstructionPortal.Api.Endpoints;

public static class AlertEndpoints
{
    public static void MapAlerts(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/alerts", async (int projectId, AppDbContext db) =>
        {
            var alerts = await db.Alerts
                .Where(a => a.ProjectId == projectId && !a.Resolved)
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => new AlertResponse(a.Id, a.ProjectId, a.AlertType, a.Severity, a.Message, a.Resolved, a.CreatedAt.ToString("O")))
                .ToListAsync();

            return Results.Ok(alerts);
        }).RequireAuthorization();

        // Automation/admin: evaluate alerts for all projects or one project
        app.MapPost("/api/alerts/evaluate", async (int? projectId, AppDbContext db, AlertRulesService rules) =>
        {
            var projectIds = projectId.HasValue
                ? new List<int> { projectId.Value }
                : await db.Projects.Select(p => p.Id).ToListAsync();

            int created = 0;

            foreach (var pid in projectIds)
            {
                var newAlerts = await rules.EvaluateProjectAsync(pid);

                // Avoid duplicate active alerts of same type with same message
                foreach (var a in newAlerts)
                {
                    var exists = await db.Alerts.AnyAsync(x =>
                        x.ProjectId == a.ProjectId &&
                        !x.Resolved &&
                        x.AlertType == a.AlertType &&
                        x.Message == a.Message
                    );

                    if (!exists)
                    {
                        db.Alerts.Add(a);
                        created++;
                    }
                }
            }

            await db.SaveChangesAsync();
            return Results.Ok(new { Created = created });
        }).RequireAuthorization(policyNames: new[] { "AdminOnly" });
    }
}

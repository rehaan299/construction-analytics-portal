using System.Security.Claims;
using ConstructionPortal.Api.Data;
using ConstructionPortal.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ConstructionPortal.Api.Endpoints;

public static class ProjectEndpoints
{
    public static void MapProjects(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/projects", async (AppDbContext db, ClaimsPrincipal user) =>
        {
            // Prototype project assignment rules:
            // FieldUser sees only ALPHA + BRAVO. Others see all.
            var role = user.FindFirstValue(ClaimTypes.Role) ?? "";
            var q = db.Projects.AsQueryable();

            if (role == "FieldUser")
                q = q.Where(p => p.Code == "ALPHA" || p.Code == "BRAVO");

            var projects = await q
                .OrderBy(p => p.Code)
                .Select(p => new ProjectResponse(
                    p.Id, p.Code, p.Name,
                    p.StartDate.ToString("yyyy-MM-dd"),
                    p.EndDatePlanned.ToString("yyyy-MM-dd"),
                    p.Budget
                ))
                .ToListAsync();

            return Results.Ok(projects);
        }).RequireAuthorization();

        app.MapGet("/api/projects/{id:int}/kpis", async (int id, AppDbContext db) =>
        {
            var project = await db.Projects.FirstAsync(p => p.Id == id);

            var actualCost = await db.CostEntries
                .Where(c => c.ProjectId == id)
                .SumAsync(c => (decimal?)c.Amount) ?? 0m;

            var budgetUsedPct = project.Budget <= 0 ? 0m : actualCost / project.Budget;

            var latestProgress = await db.DailyFieldReports
                .Where(r => r.ProjectId == id)
                .OrderByDescending(r => r.ReportDate)
                .Select(r => (int?)r.ProgressPercent)
                .FirstOrDefaultAsync() ?? 0;

            var since = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7));

            var last7Labor = await db.DailyFieldReports
                .Where(r => r.ProjectId == id && r.ReportDate >= since)
                .SumAsync(r => (int?)r.LaborHours) ?? 0;

            var last7Cost = await db.CostEntries
                .Where(c => c.ProjectId == id && c.CostDate >= since)
                .SumAsync(c => (decimal?)c.Amount) ?? 0m;

            var activeAlerts = await db.Alerts
                .CountAsync(a => a.ProjectId == id && !a.Resolved);

            return Results.Ok(new ProjectKpisResponse(
                id, project.Budget, actualCost, budgetUsedPct, latestProgress, last7Labor, last7Cost, activeAlerts
            ));
        }).RequireAuthorization();
    }
}

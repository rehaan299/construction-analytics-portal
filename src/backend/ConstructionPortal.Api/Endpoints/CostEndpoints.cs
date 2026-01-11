using ConstructionPortal.Api.Data;
using ConstructionPortal.Api.Dtos;
using ConstructionPortal.Api.Models;
using ConstructionPortal.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace ConstructionPortal.Api.Endpoints;

public static class CostEndpoints
{
    public static void MapCosts(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/costs", async (int projectId, AppDbContext db) =>
        {
            var costs = await db.CostEntries
                .Where(c => c.ProjectId == projectId)
                .OrderByDescending(c => c.CostDate)
                .Take(200)
                .Select(c => new CostEntryResponse(
                    c.Id, c.ProjectId, c.CostDate.ToString("yyyy-MM-dd"),
                    c.CostCode, c.Amount, c.Description, c.Source, c.CreatedAt.ToString("O")
                ))
                .ToListAsync();

            return Results.Ok(costs);
        }).RequireAuthorization();

        // Admin endpoint: import costs (simulated ERP push)
        app.MapPost("/api/costs/import", async (List<ImportCostRequest> req, AppDbContext db, ErpSyncService erp) =>
        {
            var entries = new List<CostEntry>();

            foreach (var r in req)
            {
                if (!DateOnly.TryParse(r.CostDate, out var d))
                    return Results.BadRequest("Invalid CostDate");

                entries.Add(new CostEntry
                {
                    ProjectId = r.ProjectId,
                    CostDate = d,
                    CostCode = r.CostCode,
                    Amount = r.Amount,
                    Description = r.Description,
                    Source = string.IsNullOrWhiteSpace(r.Source) ? "ERP_SIM" : r.Source
                });
            }

            await erp.ImportCostsAsync(entries);
            return Results.Ok(new { Imported = entries.Count });
        }).RequireAuthorization(policyNames: new[] { "AdminOnly" });
    }
}

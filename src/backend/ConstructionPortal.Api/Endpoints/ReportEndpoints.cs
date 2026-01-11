 using System.Security.Claims;
using ConstructionPortal.Api.Data;
using ConstructionPortal.Api.Dtos;
using ConstructionPortal.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionPortal.Api.Endpoints;

public static class ReportEndpoints
{
    public static void MapReports(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/reports", async (CreateDailyReportRequest req, AppDbContext db, ClaimsPrincipal user) =>
        {
            if (!DateOnly.TryParse(req.ReportDate, out var date))
                return Results.BadRequest("Invalid ReportDate");

            if (date > DateOnly.FromDateTime(DateTime.UtcNow))
                return Results.BadRequest("ReportDate cannot be in the future");

            if (req.LaborHours < 0 || req.LaborHours > 2000) return Results.BadRequest("LaborHours out of range");
            if (req.EquipmentHours < 0 || req.EquipmentHours > 2000) return Results.BadRequest("EquipmentHours out of range");
            if (req.ProgressPercent < 0 || req.ProgressPercent > 100) return Results.BadRequest("ProgressPercent out of range");

            var submittedBy = user.Identity?.Name ?? "unknown";

            var entity = new DailyFieldReport
            {
                ProjectId = req.ProjectId,
                ReportDate = date,
                LaborHours = req.LaborHours,
                EquipmentHours = req.EquipmentHours,
                ProgressPercent = req.ProgressPercent,
                Notes = req.Notes,
                SubmittedBy = submittedBy
            };

            db.DailyFieldReports.Add(entity);
            await db.SaveChangesAsync();

            return Results.Ok(new DailyReportResponse(
                entity.Id, entity.ProjectId, entity.ReportDate.ToString("yyyy-MM-dd"),
                entity.LaborHours, entity.EquipmentHours, entity.ProgressPercent,
                entity.Notes, entity.SubmittedBy, entity.CreatedAt.ToString("O")
            ));
        }).RequireAuthorization();

        app.MapGet("/api/reports", async (int projectId, AppDbContext db) =>
        {
            var reports = await db.DailyFieldReports
                .Where(r => r.ProjectId == projectId)
                .OrderByDescending(r => r.ReportDate)
                .Take(100)
                .Select(r => new DailyReportResponse(
                    r.Id, r.ProjectId, r.ReportDate.ToString("yyyy-MM-dd"),
                    r.LaborHours, r.EquipmentHours, r.ProgressPercent,
                    r.Notes, r.SubmittedBy, r.CreatedAt.ToString("O")
                ))
                .ToListAsync();

            return Results.Ok(reports);
        }).RequireAuthorization();
    }
}

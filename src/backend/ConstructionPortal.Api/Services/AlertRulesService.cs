using ConstructionPortal.Api.Data;
using ConstructionPortal.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionPortal.Api.Services;

public class AlertRulesService(IConfiguration config, AppDbContext db)
{
    public async Task<List<Alert>> EvaluateProjectAsync(int projectId)
    {
        var rules = config.GetSection("AlertRules");
        var dailyCostSpikeThreshold = rules.GetValue<decimal>("DailyCostSpikeThreshold", 50000);
        var warnPct = rules.GetValue<decimal>("BudgetBurnWarningPercent", 0.75m);
        var critPct = rules.GetValue<decimal>("BudgetBurnCriticalPercent", 0.90m);

        var project = await db.Projects.FirstAsync(p => p.Id == projectId);

        var actualCost = await db.CostEntries
            .Where(c => c.ProjectId == projectId)
            .SumAsync(c => (decimal?)c.Amount) ?? 0m;

        var budgetUsed = project.Budget <= 0 ? 0m : actualCost / project.Budget;

        var alerts = new List<Alert>();

        // Rule: Budget burn
        if (budgetUsed >= critPct)
        {
            alerts.Add(new Alert {
                ProjectId = projectId,
                AlertType = "BudgetBurnRisk",
                Severity = "Critical",
                Message = $"Budget burn critical: {budgetUsed:P0} used (Actual ${actualCost:n0} / Budget ${project.Budget:n0})."
            });
        }
        else if (budgetUsed >= warnPct)
        {
            alerts.Add(new Alert {
                ProjectId = projectId,
                AlertType = "BudgetBurnRisk",
                Severity = "Warning",
                Message = $"Budget burn warning: {budgetUsed:P0} used (Actual ${actualCost:n0} / Budget ${project.Budget:n0})."
            });
        }

        // Rule: Daily cost spike (today)
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var todayCost = await db.CostEntries
            .Where(c => c.ProjectId == projectId && c.CostDate == today)
            .SumAsync(c => (decimal?)c.Amount) ?? 0m;

        if (todayCost >= dailyCostSpikeThreshold)
        {
            alerts.Add(new Alert {
                ProjectId = projectId,
                AlertType = "DailyCostSpike",
                Severity = "Warning",
                Message = $"Daily cost spike: ${todayCost:n0} recorded for {today:yyyy-MM-dd}."
            });
        }

        // Rule: Productivity risk proxy (last 7 days labor rising + progress flat)
        var since = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7));
        var recentReports = await db.DailyFieldReports
            .Where(r => r.ProjectId == projectId && r.ReportDate >= since)
            .OrderBy(r => r.ReportDate)
            .ToListAsync();

        if (recentReports.Count >= 3)
        {
            var labor = recentReports.Sum(r => r.LaborHours);
            var progressDelta = recentReports.Last().ProgressPercent - recentReports.First().ProgressPercent;

            if (labor >= 300 && progressDelta <= 1)
            {
                alerts.Add(new Alert {
                    ProjectId = projectId,
                    AlertType = "ProductivityRisk",
                    Severity = "Info",
                    Message = $"Productivity risk proxy: {labor} labor hours in last 7 days with progress change of {progressDelta}%."
                });
            }
        }

        return alerts;
    }
}

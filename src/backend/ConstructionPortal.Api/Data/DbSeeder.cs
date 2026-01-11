using ConstructionPortal.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionPortal.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.MigrateAsync();

        if (!await db.Projects.AnyAsync())
        {
            db.Projects.AddRange(
                new Project {
                    Code = "ALPHA",
                    Name = "Project Alpha - Tower Build",
                    StartDate = new DateOnly(2026, 1, 1),
                    EndDatePlanned = new DateOnly(2026, 12, 31),
                    Budget = 5_000_000m
                },
                new Project {
                    Code = "BRAVO",
                    Name = "Project Bravo - Logistics Yard Expansion",
                    StartDate = new DateOnly(2026, 2, 1),
                    EndDatePlanned = new DateOnly(2026, 10, 15),
                    Budget = 2_200_000m
                },
                new Project {
                    Code = "CHARLIE",
                    Name = "Project Charlie - Healthcare Retrofit",
                    StartDate = new DateOnly(2026, 3, 10),
                    EndDatePlanned = new DateOnly(2026, 9, 30),
                    Budget = 1_400_000m
                }
            );
            await db.SaveChangesAsync();
        }

        if (!await db.CostEntries.AnyAsync())
        {
            var alpha = await db.Projects.FirstAsync(p => p.Code == "ALPHA");
            var bravo = await db.Projects.FirstAsync(p => p.Code == "BRAVO");

            db.CostEntries.AddRange(
                new CostEntry { ProjectId = alpha.Id, CostDate = new DateOnly(2026,1,5), CostCode="LABOR", Amount=18000, Description="Initial labor mobilization", Source="ERP_SIM" },
                new CostEntry { ProjectId = alpha.Id, CostDate = new DateOnly(2026,1,6), CostCode="MATERIAL_CONCRETE", Amount=42000, Description="Concrete pour batch 1", Source="ERP_SIM" },
                new CostEntry { ProjectId = bravo.Id, CostDate = new DateOnly(2026,2,3), CostCode="EQUIPMENT", Amount=9500, Description="Excavator rental", Source="ERP_SIM" }
            );
            await db.SaveChangesAsync();
        }

        if (!await db.DailyFieldReports.AnyAsync())
        {
            var alpha = await db.Projects.FirstAsync(p => p.Code == "ALPHA");

            db.DailyFieldReports.AddRange(
                new DailyFieldReport { ProjectId = alpha.Id, ReportDate = new DateOnly(2026,1,5), LaborHours=96, EquipmentHours=18, ProgressPercent=2, Notes="Site setup + safety orientation", SubmittedBy="field1" },
                new DailyFieldReport { ProjectId = alpha.Id, ReportDate = new DateOnly(2026,1,6), LaborHours=120, EquipmentHours=22, ProgressPercent=3, Notes="Concrete prep; minor weather delay", SubmittedBy="field1" }
            );
            await db.SaveChangesAsync();
        }
    }
}

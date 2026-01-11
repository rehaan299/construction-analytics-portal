using ConstructionPortal.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionPortal.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<DailyFieldReport> DailyFieldReports => Set<DailyFieldReport>();
    public DbSet<CostEntry> CostEntries => Set<CostEntry>();
    public DbSet<Alert> Alerts => Set<Alert>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>()
            .HasIndex(p => p.Code)
            .IsUnique();

        modelBuilder.Entity<DailyFieldReport>()
            .HasIndex(r => new { r.ProjectId, r.ReportDate });

        modelBuilder.Entity<CostEntry>()
            .HasIndex(c => new { c.ProjectId, c.CostDate });

        modelBuilder.Entity<Alert>()
            .HasIndex(a => new { a.ProjectId, a.Resolved });
    }
}

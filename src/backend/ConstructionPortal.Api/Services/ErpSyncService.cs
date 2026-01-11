using ConstructionPortal.Api.Data;
using ConstructionPortal.Api.Models;

namespace ConstructionPortal.Api.Services;

public class ErpSyncService(AppDbContext db)
{
    // Simulated ERP sync: in real enterprise, this would call ERP web services or ingest exports.
    public async Task<int> ImportCostsAsync(IEnumerable<CostEntry> entries)
    {
        db.CostEntries.AddRange(entries);
        return await db.SaveChangesAsync();
    }
}

using Microsoft.EntityFrameworkCore;
using OmgvaPOS.TaxManagement.Models;

namespace OmgvaPOS.Database.Context.MockDataHelpers;

public static class MockTaxesDataHelper
{
    
    public static async Task InitializeMockTaxesAsync(OmgvaDbContext dbContext, ILogger logger)
    {
        logger.LogDebug("Adding mock taxes...");
        await dbContext.Taxes.AddRangeAsync(MockTaxes());
        await dbContext.SaveChangesAsync();
        logger.LogDebug("Mock taxes added.");
    }
    
    public static async Task RemoveAllTaxAsync(OmgvaDbContext dbContext, ILogger logger)
    {
        logger.LogDebug("Removing all taxes...");
        var allTaxes = await dbContext.Taxes.ToListAsync();
        dbContext.Taxes.RemoveRange(allTaxes);
        await dbContext.SaveChangesAsync();
        logger.LogDebug("All taxes removed.");
    }

    private static IEnumerable<Tax> MockTaxes()
    {
        return new List<Tax>
        {
            new() { TaxType = "VAT", Percent = 20, IsArchived = false },
            new() { TaxType = "Service Tax", Percent = 5, IsArchived = false },
            new() { TaxType = "Sales Tax", Percent = 10, IsArchived = true }
        };
    }
}
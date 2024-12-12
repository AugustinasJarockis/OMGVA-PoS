using OmgvaPOS.TaxManagement.Models;

namespace OmgvaPOS.Database.Context.MockDataHelpers;

public static class MockTaxesDataHelper
{
    
    public static void InitializeMockTaxes(OmgvaDbContext dbContext, ILogger logger)
    {
        logger.LogDebug("Adding mock taxes...");
        dbContext.Taxes.AddRange(MockTaxes());
        dbContext.SaveChanges();
        logger.LogDebug("Mock taxes added.");
    }
    
    public static void RemoveAllTax(OmgvaDbContext dbContext, ILogger logger)
    {
        logger.LogDebug("Removing all taxes...");
        var allTaxes = dbContext.Taxes.ToList();
        dbContext.Taxes.RemoveRange(allTaxes);
        dbContext.SaveChanges();
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
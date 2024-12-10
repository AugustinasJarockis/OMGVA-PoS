using Microsoft.EntityFrameworkCore;
using OmgvaPOS.BusinessManagement.Models;

namespace OmgvaPOS.Database.Context.MockDataHelpers;

public static class MockBusinessesDataHelper
{
    public static long OmgvaBusinessId = 0;
    public static long DiffBusinessId = 0;
    
    public static async Task InitializeMockBusinessesAsync(OmgvaDbContext dbContext, ILogger logger)
    {
        logger.LogDebug("Adding mock businesses...");
        var omgvaBusiness = new Business
        {
            StripeAccId = "null_stripe_acc_id",
            Name = "Omgva Business",
            Address = "Address 1",
            Phone = "123456789",
            Email = "info@omgva.com"
        };
        var diffBusiness = new Business
        {
            StripeAccId = "null_stripe_acc_id",
            Name = "Diff Business",
            Address = "Address 2",
            Phone = "987654321",
            Email = "info@diffbusiness.com"
        };
        await dbContext.Businesses.AddAsync(omgvaBusiness);
        await dbContext.Businesses.AddAsync(diffBusiness);
        await dbContext.SaveChangesAsync();
        
        OmgvaBusinessId = omgvaBusiness.Id;
        DiffBusinessId = diffBusiness.Id;
        logger.LogDebug("Mock businesses added.");
    }
    
    public static async Task RemoveAllBusinessesAsync(OmgvaDbContext dbContext, ILogger logger)
    {
        logger.LogDebug("Removing all businesses...");
        var allBusinesses = await dbContext.Businesses.ToListAsync();
        dbContext.Businesses.RemoveRange(allBusinesses);
        await dbContext.SaveChangesAsync();
        logger.LogDebug("All businesses removed.");
    }

}
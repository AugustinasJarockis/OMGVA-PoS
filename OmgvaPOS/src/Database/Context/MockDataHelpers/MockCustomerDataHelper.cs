
using OmgvaPOS.CustomerManagement.Models;

namespace OmgvaPOS.Database.Context.MockDataHelpers;

public static class MockCustomerDataHelper
{

    public static void InitializeMockCustomers(OmgvaDbContext dbContext, ILogger logger)
    {
        logger.LogDebug("Adding mock customers...");
        dbContext.Customers.AddRange(MockCustomers());
        dbContext.SaveChanges();
        logger.LogDebug("Mock customers added.");
    }

    public static void RemoveAllCustomers(OmgvaDbContext dbContext, ILogger logger)
    {
        logger.LogDebug("Removing all customers...");
        var allCustomers = dbContext.Customers.ToList();
        dbContext.Customers.RemoveRange(allCustomers);
        dbContext.SaveChanges();
        logger.LogDebug("All customers removed.");
    }

    private static IEnumerable<Customer> MockCustomers()
    {
        return new List<Customer>
        {
            new() { Name = "Algimantukas" },
        };
    }
}
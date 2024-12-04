using Microsoft.EntityFrameworkCore;
using OMGVA_PoS.Data_layer.Models;

namespace OMGVA_PoS.Data_layer.Context;

public enum DbInitializerAction
{
    DO_NOTHING,
    RESET_DATABASE,
    REMOVE_ALL_DATA,
    INITIALIZE_MOCK_DATA
}

public class DbInitializer
{
    private readonly OMGVADbContext _context;
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(OMGVADbContext context, ILogger<DbInitializer> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InitDb(DbInitializerAction dbInitializerAction)
    {
        _logger.LogInformation("Initializing database with action: {Action}", dbInitializerAction);

        try
        {
            Task actionTask = dbInitializerAction switch
            {
                DbInitializerAction.DO_NOTHING => Task.CompletedTask,
                DbInitializerAction.RESET_DATABASE => ResetDatabaseAsync(),
                DbInitializerAction.REMOVE_ALL_DATA => RemoveAllDataAsync(),
                DbInitializerAction.INITIALIZE_MOCK_DATA => InitializeMockDataAsync(),
                _ => throw new ArgumentException("Invalid DbInitializerAction", nameof(dbInitializerAction))
            };

            await actionTask;
            _logger.LogInformation("Database initialization completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during database initialization.");
            throw;
        }
    }

    private async Task ResetDatabaseAsync()
    {
        _logger.LogDebug("Resetting database...");
        await RemoveAllDataAsync();
        await InitializeMockDataAsync();
    }

    private async Task RemoveAllDataAsync()
    {
        _logger.LogDebug("Removing all data from the database...");
        await RemoveAllTaxAsync();
        _logger.LogDebug("All data removed");
    }

    private async Task InitializeMockDataAsync()
    {
        _logger.LogDebug("Initializing mock data...");
        await InitializeTaxesAsync();
        _logger.LogDebug("Mock data initialized");
    }

    private async Task RemoveAllTaxAsync()
    {
        _logger.LogDebug("Removing all taxes...");
        var allTaxes = await _context.Taxes.ToListAsync();
        _context.Taxes.RemoveRange(allTaxes);
        await _context.SaveChangesAsync();
        _logger.LogInformation("All taxes removed.");
    }

    private async Task InitializeTaxesAsync()
    {
        _logger.LogDebug("Adding mock taxes...");
        await _context.AddRangeAsync(MockTax());
        await _context.SaveChangesAsync();
        _logger.LogInformation("Mock taxes added.");
    }
    
    private IEnumerable<Tax> MockTax()
    {
        return new List<Tax>
        {
            new() { TaxType = "VAT", Percent = 20, IsArchived = false },
            new() { TaxType = "Service Tax", Percent = 5, IsArchived = false },
            new() { TaxType = "Sales Tax", Percent = 10, IsArchived = true }
        };
    }
}

using OmgvaPOS.Database.Context.MockDataHelpers;

namespace OmgvaPOS.Database.Context;

public class DbInitializer
{
    private readonly OmgvaDbContext _context;
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(OmgvaDbContext context, ILogger<DbInitializer> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void InitDb(DbInitializerAction dbInitializerAction)
    {
        _logger.LogInformation("Initializing database with action: {Action}", dbInitializerAction);

        try
        {
            switch (dbInitializerAction)
            {
                case DbInitializerAction.DoNothing:
                    break;
                case DbInitializerAction.ResetDatabaseData:
                    ResetDatabaseData();
                    break;
                case DbInitializerAction.RemoveAllData:
                    RemoveAllData();
                    break;
                case DbInitializerAction.SeedMockData:
                    InitializeMockData();
                    break;
                default:
                    throw new ArgumentException("Invalid DbInitializerAction", nameof(dbInitializerAction));
            }

            _logger.LogInformation("Database initialization completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during database initialization.");
            throw;
        }
    }

    private void ResetDatabaseData()
    {
        _logger.LogInformation("Resetting database data...");
        RemoveAllData();
        InitializeMockData();
        _logger.LogInformation("Resetting database data complete...");
    }

    private void InitializeMockData()
    {
        _logger.LogInformation("Initializing mock data...");
        MockBusinessesDataHelper.InitializeMockBusinesses(_context, _logger);
        MockUserDataHelper.InitializeMockUsers(_context, _logger);
        MockTaxesDataHelper.InitializeMockTaxes(_context, _logger);
        MockItemDataHelper.InitializeMockItems(_context, _logger);
        _logger.LogInformation("Mock data initialized");
    }

    private void RemoveAllData()
    {
        _logger.LogInformation("Removing all data from the database...");
        MockTaxesDataHelper.RemoveAllTax(_context, _logger);
        MockItemDataHelper.RemoveAllItems(_context, _logger);
        MockUserDataHelper.RemoveAllUsers(_context, _logger);
        MockBusinessesDataHelper.RemoveAllBusinesses(_context, _logger);
        _logger.LogInformation("All data removed");
    }

}

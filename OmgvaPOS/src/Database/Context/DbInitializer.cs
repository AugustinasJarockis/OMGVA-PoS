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

    public async Task InitDb(DbInitializerAction dbInitializerAction)
    {
        _logger.LogInformation("Initializing database with action: {Action}", dbInitializerAction);

        try
        {
            Task actionTask = dbInitializerAction switch
            {
                DbInitializerAction.DoNothing => Task.CompletedTask,
                DbInitializerAction.ResetDatabaseData => ResetDatabaseDataAsync(),
                DbInitializerAction.RemoveAllData => RemoveAllDataAsync(),
                DbInitializerAction.SeedMockData => InitializeMockDataAsync(),
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

    private async Task ResetDatabaseDataAsync()
    {
        _logger.LogInformation("Resetting database data...");
        await RemoveAllDataAsync();
        await InitializeMockDataAsync();
        _logger.LogInformation("Resetting database data complete...");
    }

    private async Task InitializeMockDataAsync()
    {
        _logger.LogInformation("Initializing mock data...");
        await MockBusinessesDataHelper.InitializeMockBusinessesAsync(_context, _logger);
        await MockUserDataHelper.InitializeMockUsersAsync(_context, _logger);
        await MockTaxesDataHelper.InitializeMockTaxesAsync(_context, _logger);
        _logger.LogInformation("Mock data initialized");
    }

    private async Task RemoveAllDataAsync()
    {
        _logger.LogInformation("Removing all data from the database...");
        await MockUserDataHelper.RemoveAllUsersAsync(_context, _logger);
        await MockTaxesDataHelper.RemoveAllTaxAsync(_context, _logger);
        await MockBusinessesDataHelper.RemoveAllBusinessesAsync(_context, _logger);
        _logger.LogInformation("All data removed");
    }

}

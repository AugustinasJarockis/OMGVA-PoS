namespace OmgvaPOS.Database.Context;

public enum DbInitializerAction
{
    DoNothing,
    ResetDatabaseData,
    RemoveAllData,
    SeedMockData
}
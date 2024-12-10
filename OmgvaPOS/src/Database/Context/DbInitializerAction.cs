namespace OmgvaPOS.Database.Context;

public enum DbInitializerAction
{
    DO_NOTHING,
    RESET_DATABASE,
    REMOVE_ALL_DATA,
    INITIALIZE_MOCK_DATA
}
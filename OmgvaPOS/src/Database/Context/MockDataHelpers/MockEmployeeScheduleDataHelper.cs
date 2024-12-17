using OmgvaPOS.ScheduleManagement.Models;

namespace OmgvaPOS.Database.Context.MockDataHelpers;

public static class MockEmployeeScheduleDataHelper
{

    public static void InitializeMockSchedules(OmgvaDbContext dbContext, ILogger logger)
    {
        logger.LogDebug("Adding mock employee schedules...");
        dbContext.EmployeeSchedules.AddRange(MockSchedules());
        dbContext.SaveChanges();
        logger.LogDebug("Mock employee schedules added.");
    }

    public static void RemoveAllSchedules(OmgvaDbContext dbContext, ILogger logger)
    {
        logger.LogDebug("Removing all employee schedules...");
        var allSchedules = dbContext.EmployeeSchedules.ToList();
        dbContext.EmployeeSchedules.RemoveRange(allSchedules);
        dbContext.SaveChanges();
        logger.LogDebug("All employee schedules removed.");
    }

    private static IEnumerable<EmployeeSchedule> MockSchedules()
    {
        return new List<EmployeeSchedule>
        {
            new() { EmployeeId = 3, Date = DateOnly.Parse("2024-12-24"), StartTime = TimeSpan.Parse("08:00:00"), EndTime = TimeSpan.Parse("17:00:00"), IsCancelled = true },
            new() { EmployeeId = 3, Date = DateOnly.Parse("2024-12-20"), StartTime = TimeSpan.Parse("08:00:00"), EndTime = TimeSpan.Parse("17:00:00"), IsCancelled = false },
            new() { EmployeeId = 3, Date = DateOnly.Parse("2024-12-21"), StartTime = TimeSpan.Parse("10:00:00"), EndTime = TimeSpan.Parse("19:00:00"), IsCancelled = false },
        };
    }
}
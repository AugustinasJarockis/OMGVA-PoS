using OmgvaPOS.ScheduleManagement.Models;

namespace OmgvaPOS.Database.Context.MockDataHelpers;

public static class MockEmployeeScheduleDataHelper
{

    public static void InitializeMockSchedules(OmgvaDbContext dbContext, ILogger logger)
    {
        logger.LogDebug("Adding mock employee schedules...");
        var employee = dbContext.Users.FirstOrDefault(u => u.Role == 0);
        dbContext.EmployeeSchedules.AddRange(MockSchedules(employee.Id));
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

    private static IEnumerable<EmployeeSchedule> MockSchedules(long employeeId)
    {
        return new List<EmployeeSchedule>
        {
            new() { EmployeeId = employeeId, Date = DateOnly.Parse("2024-12-24"), StartTime = TimeSpan.Parse("08:00:00"), EndTime = TimeSpan.Parse("17:00:00"), IsCancelled = true },
            new() { EmployeeId = employeeId, Date = DateOnly.Parse("2024-12-20"), StartTime = TimeSpan.Parse("08:00:00"), EndTime = TimeSpan.Parse("17:00:00"), IsCancelled = false },
            new() { EmployeeId = employeeId, Date = DateOnly.Parse("2024-12-21"), StartTime = TimeSpan.Parse("10:00:00"), EndTime = TimeSpan.Parse("19:00:00"), IsCancelled = false },
        };
    }
}
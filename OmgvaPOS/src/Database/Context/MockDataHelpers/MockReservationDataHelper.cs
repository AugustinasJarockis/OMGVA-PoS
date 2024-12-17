
using OmgvaPOS.ReservationManagement.Enums;
using OmgvaPOS.ReservationManagement.Models;

namespace OmgvaPOS.Database.Context.MockDataHelpers;

public static class MockReservationDataHelper
{

    public static void InitializeMockReservations(OmgvaDbContext dbContext, ILogger logger)
    {
        logger.LogDebug("Adding mock reservations...");
        var employee = dbContext.Users.FirstOrDefault(u => u.Role == 0);
        var customer = dbContext.Customers.FirstOrDefault();
        var item = dbContext.Items.FirstOrDefault(i => i.Duration != null);
        dbContext.Reservations.AddRange(MockReservations(employee.Id, customer.Id, item.Id));
        dbContext.SaveChanges();
        logger.LogDebug("Mock reservations added.");
    }

    public static void RemoveAllReservations(OmgvaDbContext dbContext, ILogger logger)
    {
        logger.LogDebug("Removing all reservations...");
        var allReservations = dbContext.Reservations.ToList();
        dbContext.Reservations.RemoveRange(allReservations);
        dbContext.SaveChanges();
        logger.LogDebug("All reservations removed.");
    }

    private static IEnumerable<Reservation> MockReservations(long employeeId, long customerId, long itemId)
    {
        return new List<Reservation>
        {
            new() { TimeCreated = DateTime.UtcNow, TimeReserved = DateTime.Parse("2024-12-20T14:00:00"), Status = ReservationStatus.Open, EmployeeId = employeeId, CustomerId = customerId, ItemId = itemId },
            new() { TimeCreated = DateTime.UtcNow, TimeReserved = DateTime.Parse("2024-12-20T15:00:00"), Status = ReservationStatus.Cancelled, EmployeeId = employeeId, CustomerId = customerId, ItemId = itemId },
            new() { TimeCreated = DateTime.UtcNow, TimeReserved = DateTime.Parse("2024-12-19T13:00:00"), Status = ReservationStatus.Done, EmployeeId = employeeId, CustomerId = customerId, ItemId = itemId },
        };
    }
}
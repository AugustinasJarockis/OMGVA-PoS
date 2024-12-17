
using OmgvaPOS.ReservationManagement.Enums;
using OmgvaPOS.ReservationManagement.Models;

namespace OmgvaPOS.Database.Context.MockDataHelpers;

public static class MockReservationDataHelper
{

    public static void InitializeMockReservations(OmgvaDbContext dbContext, ILogger logger)
    {
        logger.LogDebug("Adding mock reservations...");
        dbContext.Reservations.AddRange(MockReservations());
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

    private static IEnumerable<Reservation> MockReservations()
    {
        return new List<Reservation>
        {
            new() { TimeCreated = DateTime.UtcNow, TimeReserved = DateTime.Parse("2024-12-20T14:00:00"), Status = ReservationStatus.Open, EmployeeId = 3, CustomerId = 1, ItemId = 3 },
            new() { TimeCreated = DateTime.UtcNow, TimeReserved = DateTime.Parse("2024-12-20T15:00:00"), Status = ReservationStatus.Cancelled, EmployeeId = 3, CustomerId = 1, ItemId = 3 },
            new() { TimeCreated = DateTime.UtcNow, TimeReserved = DateTime.Parse("2024-12-19T13:00:00"), Status = ReservationStatus.Done, EmployeeId = 3, CustomerId = 1, ItemId = 3 },
        };
    }
}
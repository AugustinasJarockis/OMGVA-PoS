namespace OmgvaPOS.ReservationManagement.DTOs;

public class CreateReservationRequest
{
    public DateTime TimeReserved { get; set; }
    public long EmployeeId { get; set; }
    public long CustomerId { get; set; }
    public long ItemId { get; set; }
}
namespace OmgvaPOS.ReservationManagement.DTOs;

public class CreateReservationDto
{
    public DateTime TimeReserved { get; set; }
    public long EmployeeId { get; set; }
    public long CustomerId { get; set; }
}
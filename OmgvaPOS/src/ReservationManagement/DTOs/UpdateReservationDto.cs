using OmgvaPOS.ReservationManagement.Enums;

namespace OmgvaPOS.ReservationManagement.DTOs;

public class UpdateReservationDto
{
    public DateTime TimeReserved { get; set; }
    public ReservationStatus Status { get; set; }
    public long EmployeeId { get; set; }
}
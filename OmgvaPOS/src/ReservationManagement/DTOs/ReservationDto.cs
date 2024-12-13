using OmgvaPOS.ReservationManagement.Enums;

namespace OmgvaPOS.ReservationManagement.DTOs;

public class ReservationDto
{
    public long Id { get; set; }
    public DateTime TimeCreated { get; set; }
    public DateTime TimeReserved { get; set; }
    public ReservationStatus Status { get; set; }
    public long EmployeeId { get; set; }
    public long CustomerId { get; set; }
        
    // Additional fields that might be useful in responses
    public string EmployeeName { get; set; }
    public string CustomerName { get; set; }
}
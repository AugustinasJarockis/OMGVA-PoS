using OmgvaPOS.OrderManagement.Enums;

namespace OmgvaPOS.OrderManagement.DTOs;

public class SimpleOrderDTO
{
    public long Id { get; set; }
    public OrderStatus Status { get; set; }
    public SimpleUserDTO? User { get; set; }
}

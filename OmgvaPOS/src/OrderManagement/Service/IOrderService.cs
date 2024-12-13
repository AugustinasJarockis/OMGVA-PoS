using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderManagement.DTOs;

namespace OmgvaPOS.OrderManagement.Service;

public interface IOrderService
{
    public OrderDTO CreateOrder(CreateOrderRequest request);
}

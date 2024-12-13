using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderManagement.Models;

namespace OmgvaPOS.OrderManagement.Service;

public interface IOrderService
{
    public OrderDTO CreateOrder(CreateOrderRequest request);
    public IEnumerable<OrderDTO> GetAllBusinessOrders(long businessId);
    public IEnumerable<OrderDTO> GetAllActiveOrders(long businessId);
    public OrderDTO GetOrder(long id);
    public long GetOrderBusinessId(long id);
    public void DeleteOrder(long id);
    public void AddOrderItem(long orderId, CreateOrderItemRequest request);
    public void UpdateOrderItem(long itemId, UpdateOrderItemRequest request);
    public void DeleteOrderItem(long orderId, long itemId);
}

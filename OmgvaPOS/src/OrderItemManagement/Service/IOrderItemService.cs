using OmgvaPOS.OrderItemManagement.DTOs;
using OmgvaPOS.OrderManagement.DTOs;

namespace OmgvaPOS.OrderItemManagement.Service;

public interface IOrderItemService
{
    OrderItemDTO AddOrderItem(long orderId, CreateOrderItemRequest request);
    void DeleteOrderItem(long orderItemId, bool useTransaction);
    OrderItemDTO GetOrderItem(long orderItemId);
    void UpdateOrderItem(long orderItemId, UpdateOrderItemRequest request);
}

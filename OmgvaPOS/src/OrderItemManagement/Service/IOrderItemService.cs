using OmgvaPOS.OrderItemManagement.DTOs;
using OmgvaPOS.OrderManagement.DTOs;

namespace OmgvaPOS.OrderItemManagement.Service;

public interface IOrderItemService
{
    void AddOrderItem(long orderId, CreateOrderItemRequest request);
    OrderItemDTO GetOrderItem(long orderId);
    void DeleteOrderItem(long orderItemId, bool useTransaction);
    void UpdateOrderItem(long orderItemId, UpdateOrderItemRequest request);
}

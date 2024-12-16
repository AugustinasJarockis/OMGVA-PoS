using OmgvaPOS.OrderItemManagement.Models;

namespace OmgvaPOS.OrderItemManagement.Repository;

public interface IOrderItemRepository
{
    void AddOrderItem(OrderItem orderItem);
    OrderItem GetOrderItem(long orderItemId);
    OrderItem GetOrderItemOrThrow(long orderId);
    void DeleteOrderItem(OrderItem orderItem);
    void UpdateOrderItemQuantity(OrderItem orderItem);
}

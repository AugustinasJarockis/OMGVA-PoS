using OmgvaPOS.OrderItemManagement.Models;

namespace OmgvaPOS.OrderItemManagement.Repository;

public interface IOrderItemRepository
{
    OrderItem AddOrderItem(OrderItem orderItem);
    OrderItem GetOrderItem(long orderItemId);
    void DeleteOrderItem(OrderItem orderItem);
    void UpdateOrderItemQuantity(OrderItem orderItem);
}

using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderManagement.Models;

namespace OmgvaPOS.OrderManagement.Repository;

public interface IOrderRepository
{
    public Order AddOrder(Order order);
    public Order GetOrder(long id);
    public IEnumerable<Order> GetAllBusinessOrders(long businessId);
    public void DeleteOrder(Order order);
    public OrderItem GetOrderItem(long orderItemId);
    public void DeleteOrderItem(OrderItem orderItem);
    public void AddOrderItem(OrderItem orderItem);
    public void UpdateOrderItemQuantity(OrderItem orderItem);
    public void UpdateOrderTip(Order order);
}

using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderManagement.Models;

namespace OmgvaPOS.OrderManagement.Repository;

public interface IOrderRepository
{
    public Order AddOrder(Order order);
    Order GetOrder(long id);
    public IEnumerable<Order> GetAllBusinessOrders(long businessId);
    public void RemoveOrderItem(OrderItem orderItem);
    public void DeleteOrder(Order order);
}

using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderManagement.Models;

namespace OmgvaPOS.OrderManagement.Repository;

public interface IOrderRepository
{
    Order AddOrder(Order order);
    Order GetOrder(long id);
    IEnumerable<Order> GetAllBusinessOrders(long businessId);
    void DeleteOrder(Order order);
    void UpdateOrderTip(Order order);
    long GetOrderBusinessId(long id);
}

using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderManagement.Models;

namespace OmgvaPOS.OrderManagement.Repository;

public interface IOrderRepository
{
    Order AddOrder(Order order);
    Order GetOrderNoAppendages(long orderId);
    Order GetOrder(long id);
    IEnumerable<Order> GetAllBusinessOrders(long businessId);
    void DeleteOrder(Order order);
    void UpdateOrder(Order order);
    long GetOrderBusinessId(long id);
}

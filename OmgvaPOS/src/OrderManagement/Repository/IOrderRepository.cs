using OmgvaPOS.OrderManagement.Models;

namespace OmgvaPOS.OrderManagement.Repository;

public interface IOrderRepository
{
    public Order AddOrder(Order order);
    Order GetOrder(long id);
}

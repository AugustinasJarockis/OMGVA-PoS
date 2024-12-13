using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderManagement.Models;

namespace OmgvaPOS.OrderManagement.Service;

public interface IOrderService
{
    public OrderDTO CreateOrder(CreateOrderRequest request);
    public ICollection<OrderDTO> GetAllBusinessOrders(long businessId);
    public OrderDTO GetOrder(long id);
    public long GetOrderBusinessId(long id);
}

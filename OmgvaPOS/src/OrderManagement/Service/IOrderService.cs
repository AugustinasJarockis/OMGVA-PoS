using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderManagement.Models;

namespace OmgvaPOS.OrderManagement.Service;

public interface IOrderService
{
    public OrderDTO CreateOrder(long businessId, long userId);
    public IEnumerable<OrderDTO> GetAllBusinessOrders(long businessId);
    public IEnumerable<OrderDTO> GetAllActiveOrders(long businessId);
    public OrderDTO GetOrder(long id);
    public long GetOrderBusinessId(long id);
    public void DeleteOrder(long id);
    public void UpdateOrderTip(short tip, long orderId);
}

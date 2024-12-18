using OmgvaPOS.DiscountManagement.Models;
using OmgvaPOS.OrderManagement.DTOs;

namespace OmgvaPOS.OrderManagement.Service;

public interface IOrderService
{
    public SimpleOrderDTO CreateOrder(long businessId, long userId);
    public IEnumerable<SimpleOrderDTO> GetAllBusinessOrders(long businessId);
    public IEnumerable<SimpleOrderDTO> GetAllActiveOrders(long businessId);
    public OrderDTO GetOrder(long id);
    public long GetOrderBusinessId(long id);
    public void DeleteOrder(long id);
    public void UpdateOrderTip(short tip, long orderId);
}

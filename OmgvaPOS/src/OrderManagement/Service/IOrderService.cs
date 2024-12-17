using OmgvaPOS.DiscountManagement.Models;
using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderManagement.Models;

namespace OmgvaPOS.OrderManagement.Service;

public interface IOrderService
{
    public SimpleOrderDTO CreateOrder(long businessId, long userId);
    public IEnumerable<SimpleOrderDTO> GetAllBusinessOrders(long businessId);
    public IEnumerable<SimpleOrderDTO> GetAllActiveOrders(long businessId);
    public OrderDTO GetOrder(long id);
    public long GetOrderBusinessId(long id);
    public void DeleteOrder(long id);
    public SimpleOrderDTO UpdateOrder(UpdateOrderRequest updateRequest, long orderId);
    public Order GetOrderOrThrow(long orderId);
}

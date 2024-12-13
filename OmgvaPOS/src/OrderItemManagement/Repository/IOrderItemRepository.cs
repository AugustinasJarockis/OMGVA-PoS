using Microsoft.EntityFrameworkCore;
using OmgvaPOS.OrderItemManagement.Models;

namespace OmgvaPOS.OrderItemManagement.Repository;

public interface IOrderItemRepository
{
    public void RemoveOrderItem(OrderItem orderItem);
}

using Microsoft.EntityFrameworkCore;
using OmgvaPOS.Database.Context;
using OmgvaPOS.DiscountManagement.Repository;
using OmgvaPOS.OrderItemManagement.Models;

namespace OmgvaPOS.OrderItemManagement.Repository;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly OmgvaDbContext _context;
    public OrderItemRepository(OmgvaDbContext context) {
        _context = context;
    }
    public void RemoveOrderItem(OrderItem orderItem) {
        // Mark for deletion
        _context.OrderItems.Remove(orderItem);
    }
}

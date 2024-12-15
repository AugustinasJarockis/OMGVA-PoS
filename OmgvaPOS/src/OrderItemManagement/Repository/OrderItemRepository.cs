using Microsoft.EntityFrameworkCore;
using OmgvaPOS.Database.Context;
using OmgvaPOS.DiscountManagement.Repository;
using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderItemManagement.Repository;

namespace OmgvaPOS.OrderItemManagement.Repository;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly OmgvaDbContext _context;
    private readonly ILogger<DiscountRepository> _logger;
    public OrderItemRepository(OmgvaDbContext context, ILogger<DiscountRepository> logger) {
        _context = context;
        _logger = logger;
    }

    public OrderItem AddOrderItem(OrderItem orderItem) {
        _context.OrderItems.Add(orderItem);
        _context.SaveChanges();
        return orderItem;
    }

    public void DeleteOrderItem(OrderItem orderItem) {
        var orderItemVariations = _context.OrderItemVariations
        .Where(v => v.OrderItemId == orderItem.Id)
        .ToList();

        _context.OrderItemVariations.RemoveRange(orderItemVariations);
        _context.OrderItems.Remove(orderItem);
        _context.SaveChanges();
    }

    public OrderItem GetOrderItem(long orderItemId) {
        var orderItem = _context.OrderItems
            .Include(oi => oi.OrderItemVariations) 
            .FirstOrDefault(oi => oi.Id == orderItemId);
        return orderItem;
    }

    public void UpdateOrderItemQuantity(OrderItem orderItem) {
        _context.OrderItems.Update(orderItem);
        _context.SaveChanges();
    }
}

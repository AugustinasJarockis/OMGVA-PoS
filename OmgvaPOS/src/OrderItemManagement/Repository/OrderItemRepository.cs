using Microsoft.EntityFrameworkCore;
using OmgvaPOS.Database.Context;
using OmgvaPOS.DiscountManagement.Repository;
using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderItemManagement.Repository;
using OmgvaPOS.OrderItemManagement.Validators;
using OmgvaPOS.OrderManagement.Enums;

namespace OmgvaPOS.OrderItemManagement.Repository;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly OmgvaDbContext _context;
    private readonly ILogger<DiscountRepository> _logger;
    public OrderItemRepository(OmgvaDbContext context, ILogger<DiscountRepository> logger) {
        _context = context;
        _logger = logger;
    }

    public void AddOrderItem(OrderItem orderItem) {
        _context.OrderItems.Add(orderItem);
        _context.SaveChanges();
    }

    public void DeleteOrderItem(OrderItem orderItem) {
        var orderItemFromDb = _context.OrderItems
            .Include(o => o.OrderItemVariations)
            .FirstOrDefault(o => o.Id == orderItem.Id);

        if (orderItemFromDb != null) {
            var variations = orderItemFromDb.OrderItemVariations.ToList();
            _context.OrderItemVariations.RemoveRange(variations);
            _context.OrderItems.Remove(orderItemFromDb);
            _context.SaveChanges();
        }
    }

    public OrderItem GetOrderItem(long orderItemId) {
        var orderItem = _context.OrderItems
            .Where(oi => oi.Id == orderItemId)
            .Include(oi => oi.OrderItemVariations)
            .Include(oi => oi.Discount)
            .FirstOrDefault();
        return orderItem;
    }

    public OrderItem GetOrderItemOrThrow(long orderItemId) {
        var orderItem = _context.OrderItems
            .Where(oi => oi.Id == orderItemId)
            .Include(oi => oi.OrderItemVariations)
            .Include(oi => oi.Discount)
            .FirstOrDefault();
        OrderItemValidator.Exists(orderItem);
        return orderItem;
    }

    public List<OrderItem> GetOrderItemsByItemId(long itemId) {
        return [.. _context.OrderItems
            .Where(oi => oi.ItemId == itemId)
            .Include(oi => oi.Order)
            .Where(oi => oi.Order.Status == OrderStatus.Open)
            .Include(oi => oi.OrderItemVariations)];
    }
    public void UpdateOrderItemQuantity(OrderItem orderItem) {
        _context.OrderItems.Update(orderItem);
        _context.SaveChanges();
    }
}

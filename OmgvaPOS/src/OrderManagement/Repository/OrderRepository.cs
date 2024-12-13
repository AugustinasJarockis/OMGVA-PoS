using Microsoft.EntityFrameworkCore;
using OmgvaPOS.BusinessManagement.Models;
using OmgvaPOS.Database.Context;
using OmgvaPOS.DiscountManagement.Repository;
using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderManagement.Models;

namespace OmgvaPOS.OrderManagement.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly OmgvaDbContext _context;
    private readonly ILogger<DiscountRepository> _logger;
    public OrderRepository(OmgvaDbContext context, ILogger<DiscountRepository> logger) {
        _context = context;
        _logger = logger;
    }

    public Order AddOrder(Order order) {
        try {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occured while adding a new order.");
            throw new ApplicationException("Error adding an order.");
        }
    }

    public Order GetOrder(long id) {
        try {
            var order = _context.Orders
                .Where(o => o.Id == id)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.OrderItemVariation) // Include variations
                .Include(o => o.Payment) // Include Payment if necessary
                .Include(o => o.Discount) // Include Discount if necessary
                .Include(o => o.User) // Include User if necessary
                .FirstOrDefault();
            return order;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occured while getting an order.");
            throw new ApplicationException("Error getting an order.");
        }
    }

    public IEnumerable<Order> GetAllBusinessOrders(long businessId) {
        try {
            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.OrderItemVariation) 
                .Include(o => o.Payment) 
                .Include(o => o.Discount) 
                .Include(o => o.User) 
                .Where(o => o.OrderItems.Any(oi =>
                    _context.Items.Any(i => i.Id == oi.ItemId && i.BusinessId == businessId)))
                .ToList();
            return orders;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occured while getting all orders.");
            throw new ApplicationException("Error getting all orders.");
        }
    }

    public void DeleteOrderItem(OrderItem orderItem) {
        try {
            _context.OrderItems.Remove(orderItem);
            _context.SaveChanges();
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occured while removing order item.");
            throw new ApplicationException("Error updating the order.");
        }
    }

    public void DeleteOrder(Order order) {
        try {
            foreach (var orderItem in order.OrderItems.ToList()) {
                if (orderItem.OrderItemVariation != null) {
                    _context.OrderItemVariations.Remove(orderItem.OrderItemVariation);
                }
                _context.OrderItems.Remove(orderItem);
            }
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occured while removing order item.");
            throw new ApplicationException("Error updating the order.");
        }
    }

    public void AddOrderItem(OrderItem orderItem) {
        try {
            _context.OrderItems.Add(orderItem);
            _context.SaveChanges();
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occured while adding order item.");
            throw new ApplicationException("Error adding the order.");
        }
    }
    public OrderItem GetOrderItem(long orderItemId) {
        try {
            var orderItem = _context.OrderItems.FirstOrDefault(oi => oi.Id == orderItemId);
            return orderItem;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occured while Deleting orderItem.");
            throw new ApplicationException("Error deleting orderItem.");
        }
    }

    public void UpdateOrderItemQuantity(OrderItem orderItem) {
        try {
            _context.OrderItems.Update(orderItem);
            _context.SaveChanges();
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occured while Deleting orderItem.");
            throw new ApplicationException("Error deleting orderItem.");
        }
    }

    public void UpdateOrderTip(Order order) {
        try {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occured while updating Order tip.");
            throw new ApplicationException("Error updating Order tip.");
        }
    }
}

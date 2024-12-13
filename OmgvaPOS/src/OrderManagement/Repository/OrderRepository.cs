using Microsoft.EntityFrameworkCore;
using OmgvaPOS.Database.Context;
using OmgvaPOS.DiscountManagement.Repository;
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
            _logger.LogError(ex, "An error occured while adding a new order.");
            throw new ApplicationException("Error adding an order.");
        }
    }
}

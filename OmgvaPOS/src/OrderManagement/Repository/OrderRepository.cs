using Microsoft.EntityFrameworkCore;
using OmgvaPOS.BusinessManagement.Models;
using OmgvaPOS.Database.Context;
using OmgvaPOS.DiscountManagement.Repository;
using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderManagement.Models;
using OmgvaPOS.OrderManagement.Validators;

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
        _context.Orders.Add(order);
        _context.SaveChanges();
        return order;
    }

    public long GetOrderBusinessId(long orderId) {
        var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
        OrderValidator.Exists(order);

        return order.BusinessId;
    }

    public Order GetOrder(long orderId) {
        var order = _context.Orders
            .AsNoTracking()
            .Where(o => o.Id == orderId)
            .Include(o => o.OrderItems)
            .Include(o => o.Payment)
            .Include(o => o.Discount)
            .Include(o => o.User)
            .FirstOrDefault();
        return order;
    }

    public IEnumerable<Order> GetAllBusinessOrders(long businessId) {
        var orders = _context.Orders
            .Where (o => o.BusinessId == businessId)
            .Include(o => o.User)
            .ToList();
        return orders;
    }

    public void DeleteOrder(Order order) {
        _context.Orders.Remove(order);
        _context.SaveChanges();
    }

    public void UpdateOrderTip(Order order) {
        _context.Orders.Update(order);
        _context.SaveChanges();
    }
}

using Microsoft.EntityFrameworkCore;
using OmgvaPOS.Database.Context;
using OmgvaPOS.OrderManagement.Models;
using OmgvaPOS.OrderManagement.Validators;

namespace OmgvaPOS.OrderManagement.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly OmgvaDbContext _context;
    public OrderRepository(OmgvaDbContext context) {
        _context = context;
    }


    public IQueryable<Order> GetOrderQueryable()
    {
        return _context.Orders;
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

    public Order GetOrderNoAppendages(long orderId) {
        return _context.Orders.FirstOrDefault(o => o.Id == orderId);
    }

    public Order GetOrder(long orderId) {
        var order = _context.Orders
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

    public void UpdateOrder(Order order) {
        _context.Orders.Update(order);
        _context.SaveChanges();
    }
}

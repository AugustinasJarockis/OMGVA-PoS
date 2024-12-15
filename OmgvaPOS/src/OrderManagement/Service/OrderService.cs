using Microsoft.EntityFrameworkCore;
using OmgvaPOS.Database.Context;
using OmgvaPOS.ItemManagement.Repositories;
using OmgvaPOS.ItemManagement.Services;
using OmgvaPOS.OrderItemManagement.Service;
using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderManagement.Enums;
using OmgvaPOS.OrderManagement.Mappers;
using OmgvaPOS.OrderManagement.Repository;
using OmgvaPOS.OrderManagement.Validators;

namespace OmgvaPOS.OrderManagement.Service;

public class OrderService : IOrderService
{
    private readonly OmgvaDbContext _context;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemService _orderItemService;
    private readonly IItemService _itemService;
    private readonly IItemRepository _itemRepository;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        OmgvaDbContext context,
        IOrderRepository orderRepository,
        IOrderItemService orderItemService,
        IItemService itemService,
        IItemRepository itemRepository,
        ILogger<OrderService> logger
        ) {
        _context = context;
        _orderRepository = orderRepository;
        _orderItemService = orderItemService;
        _itemService = itemService;
        _itemRepository = itemRepository;
        _logger = logger;
    }

    public long GetOrderBusinessId(long id) {
        return _orderRepository.GetOrderBusinessId(id);
    }

    public OrderDTO CreateOrder(long businessId, long userId) {
        var order = OrderMapper.RequestToOrder(businessId, userId);

        order = _orderRepository.AddOrder(order);
        return OrderMapper.OrderToDTO(order);
    }

    public OrderDTO GetOrder(long id) {
        var order = _orderRepository.GetOrder(id);
        OrderValidator.Exists(order);

        return OrderMapper.OrderToDTO(order);
    }

    public IEnumerable<OrderDTO> GetAllBusinessOrders(long businessId) {
        var orders = _orderRepository.GetAllBusinessOrders(businessId);
        OrderValidator.Exist(orders);

        return orders.Select(o => o.OrderToDTO()).ToList();
    }

    public IEnumerable<OrderDTO> GetAllActiveOrders(long businessId) {
        var orders = _orderRepository.GetAllBusinessOrders(businessId);
        var activeOrders = orders.Where(o => o.Status == OrderStatus.Open);
        OrderValidator.Exist(activeOrders);

        return activeOrders.Select(o => o.OrderToDTO()).ToList();
    }

    public void DeleteOrder(long id) {
        var order = _orderRepository.GetOrder(id);
        OrderValidator.Exists(order);
        OrderValidator.IsOpen(order);

        using var transaction = _context.Database.BeginTransaction();
        try {
            // TODO: nested transactions dont work
            // if order has order items you cannot delete the order
            // you need to delete order items first
            // it should not be like that
            foreach (var orderItem in order.OrderItems) {
                _orderItemService.DeleteOrderItem(orderItem.Id);
            }
            _orderRepository.DeleteOrder(order);

            transaction.Commit();
        }
        catch (Exception ex) {
            transaction.Rollback();

            _logger.LogError(ex, "An error occurred while deleting order.");
            throw new ApplicationException("Error deleting order. The operation has been rolled back.");
        }

    }
    

    public void UpdateOrderTip(short tip, long orderId) {
        var order = _orderRepository.GetOrder(orderId);
        OrderValidator.Exists(order);
        OrderValidator.IsOpen(order);

        order.Tip = tip;
        _orderRepository.UpdateOrderTip(order);
    }
}

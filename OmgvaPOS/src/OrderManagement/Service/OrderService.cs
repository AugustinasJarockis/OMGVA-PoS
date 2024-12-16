using Microsoft.EntityFrameworkCore;
using OmgvaPOS.Database.Context;
using OmgvaPOS.DiscountManagement.Models;
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

    public long GetOrderBusinessId(long businessId) {
        return _orderRepository.GetOrderBusinessId(businessId);
    }

    public SimpleOrderDTO CreateOrder(long businessId, long userId) {
        var order = OrderMapper.RequestToOrder(businessId, userId);

        order = _orderRepository.AddOrder(order);
        return OrderMapper.ToSimpleOrderDTO(order);
    }

    public OrderDTO GetOrder(long orderId) {
        var order = _orderRepository.GetOrder(orderId);
        OrderValidator.Exists(order);
        
        List<OrderItemDTO> orderItemDTOs = [];
        foreach(var orderItem in order.OrderItems) {
            orderItemDTOs.Add(_orderItemService.GetOrderItem(orderItem.Id));
        }

        var orderDTO = new OrderDTO {
            Id = order.Id,
            Status = order.Status,
            Tip = order.Tip,
            RefundReason = order.RefundReason,
            Discount = order.Discount.ToSimpleDiscountDTO(),
            User = order.User.ToSimpleUserDTO(),
            OrderItems = orderItemDTOs
        };

        return orderDTO;
    }

    public IEnumerable<SimpleOrderDTO> GetAllBusinessOrders(long businessId) {
        var orders = _orderRepository.GetAllBusinessOrders(businessId);
        OrderValidator.Exist(orders);

        return orders.ToSimpleOrderDTOList();
    }

    public IEnumerable<SimpleOrderDTO> GetAllActiveOrders(long businessId) {
        var orders = GetAllBusinessOrders(businessId);
        var activeOrders = orders.Where(o => o.Status == OrderStatus.Open);
        OrderValidator.Exist(activeOrders);

        return activeOrders;
    }

    public void DeleteOrder(long id) {
        var order = _orderRepository.GetOrder(id);
        OrderValidator.Exists(order);
        OrderValidator.IsOpen(order);

        using var transaction = _context.Database.BeginTransaction();
        try {
            foreach (var orderItem in order.OrderItems) {
                _orderItemService.DeleteOrderItem(orderItem.Id, false);
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
        _orderRepository.UpdateOrder(order);
    }
}

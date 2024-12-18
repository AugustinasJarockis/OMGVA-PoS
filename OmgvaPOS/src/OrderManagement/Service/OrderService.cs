﻿using OmgvaPOS.Database.Context;
using OmgvaPOS.ItemManagement.Repositories;
using OmgvaPOS.ItemManagement.Services;
using OmgvaPOS.OrderItemManagement.Service;
using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderManagement.Enums;
using OmgvaPOS.OrderManagement.Mappers;
using OmgvaPOS.OrderManagement.Models;
using OmgvaPOS.OrderManagement.Repository;
using OmgvaPOS.OrderManagement.Validators;
using OmgvaPOS.UserManagement.Service;

namespace OmgvaPOS.OrderManagement.Service;

public class OrderService : IOrderService
{
    private readonly OmgvaDbContext _context;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemService _orderItemService;
    private readonly IItemService _itemService;
    private readonly IItemRepository _itemRepository;
    private readonly IUserService _userService;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        OmgvaDbContext context,
        IOrderRepository orderRepository,
        IOrderItemService orderItemService,
        IItemService itemService,
        IItemRepository itemRepository,
        ILogger<OrderService> logger, 
        IUserService userService) {
        _context = context;
        _orderRepository = orderRepository;
        _orderItemService = orderItemService;
        _itemService = itemService;
        _itemRepository = itemRepository;
        _logger = logger;
        _userService = userService;
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

        decimal finalPrice = 0;
        decimal finalTaxesPaid = 0;

        List<OrderItemDTO> orderItemDTOs = [];
        foreach (var orderItem in order.OrderItems) {
            var orderItemDTO = _orderItemService.GetOrderItem(orderItem.Id);
            orderItemDTOs.Add(orderItemDTO);

            // order discount applies to order item only
            // in so far as it doesnt exceed existing OderItem discount
            var maxDiscountPercent = Math.Max(
                orderItemDTO.Discount?.DiscountAmount ?? 0,
                order.Discount?.Amount ?? 0
            );

            var itemFinalPrice = (orderItemDTO.UnitPriceNoDiscount * orderItemDTO.Quantity) * (100 - maxDiscountPercent) / 100;
            finalPrice += itemFinalPrice;
            finalTaxesPaid += itemFinalPrice * orderItemDTO.TaxPercent / (100 + orderItemDTO.TaxPercent);
        }

        finalPrice += order.Tip;

        var orderDTO = new OrderDTO {
            Id = order.Id,
            Status = order.Status,
            Tip = order.Tip,
            RefundReason = order.RefundReason,
            FinalPrice = finalPrice,
            TaxesPaid = finalTaxesPaid,
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

    public Order GetOrderOrThrow(long orderId)
    {
        var order = _orderRepository.GetOrder(orderId);
        OrderValidator.Exists(order);
        return order;
    }
    
    public SimpleOrderDTO UpdateOrder(UpdateOrderRequest updateRequest, long orderId)
    {
        OrderValidator.ValidateUpdateOrderRequest(updateRequest);
        var order = GetOrderOrThrow(orderId);

        if (updateRequest.UserId != null)
        {
            _userService.ValidateUserBelongsToBusiness(updateRequest.UserId, order.BusinessId);
        }
        
        var updatedOrder = updateRequest.ToUpdatedOrder(order);
        _orderRepository.UpdateOrder(order);

        if (updateRequest.Status != null)
        {
            updatedOrder = UpdateOrderStatus(orderId, updateRequest.Status.Value);
        }
        

        return updatedOrder.ToSimpleOrderDTO();
    }

    public Order UpdateOrderStatus(long orderId, OrderStatus orderStatus)
    {
        var order = GetOrderOrThrow(orderId);
        
        // TODO: any extra logic that might be needed when changing order status. 
        order.Status = orderStatus;
        _orderRepository.UpdateOrder(order);

        return order;
    }
    
}

﻿using OmgvaPOS.DiscountManagement.Mappers;
using OmgvaPOS.DiscountManagement.Models;
using OmgvaPOS.ItemManagement.Services;
using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderManagement.Mappers;
using OmgvaPOS.OrderManagement.Models;
using OmgvaPOS.OrderManagement.Repository;

namespace OmgvaPOS.OrderManagement.Service;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IItemService _itemService;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository orderRepository,
        IItemService itemService,
        ILogger<OrderService> logger
        ) {
        _orderRepository = orderRepository;
        _itemService = itemService;
        _logger = logger;
    }

    public OrderDTO CreateOrder(CreateOrderRequest request) {
        if (request.CreateOrderItemRequests == null || !request.CreateOrderItemRequests.Any())
            throw new ArgumentException("Order must have at least one item.");

        var order = OrderMappers.RequestToOrder(request);
        order = _orderRepository.AddOrder(order);

        return OrderMappers.OrderToOrderDTO(order);
    }
    public OrderDTO GetOrder(long id) {
        var order = _orderRepository.GetOrder(id);
        return OrderMappers.OrderToOrderDTO(order);
    }

    public long GetOrderBusinessId(long id) {
        var order = _orderRepository.GetOrder(id);
        if (order == null)
            throw new KeyNotFoundException($"Could not find an order with id: {id}");

        var itemId = order.OrderItems.FirstOrDefault().ItemId;
        return _itemService.GetItemNoException(itemId).BusinessId;
    }
}

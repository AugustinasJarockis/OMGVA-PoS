using Microsoft.EntityFrameworkCore;
using OmgvaPOS.DiscountManagement.Mappers;
using OmgvaPOS.DiscountManagement.Models;
using OmgvaPOS.ItemManagement.Services;
using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderManagement.Enums;
using OmgvaPOS.OrderManagement.Helpers;
using OmgvaPOS.OrderManagement.Mappers;
using OmgvaPOS.OrderManagement.Models;
using OmgvaPOS.OrderManagement.Repository;

namespace OmgvaPOS.OrderManagement.Service;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly OrderItemUpdater _orderItemUpdater;
    private readonly IItemService _itemService;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository orderRepository,
        OrderItemUpdater orderItemUpdater,
        IItemService itemService,
        ILogger<OrderService> logger
        ) {
        _orderRepository = orderRepository;
        _orderItemUpdater = orderItemUpdater;
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
    public IEnumerable<OrderDTO> GetAllBusinessOrders(long businessId) {
        var orders = _orderRepository.GetAllBusinessOrders(businessId);
        if (orders == null)
            throw new KeyNotFoundException("Could not find any business orders");
       
        return orders.Select(o => o.OrderToOrderDTO()).ToList();
    }
    public IEnumerable<OrderDTO> GetAllActiveOrders(long businessId) {
        var orders = GetAllBusinessOrders(businessId)
            .Where(o => o.Status == OrderStatus.Open);
        //TODO: should be custom exception
        if (orders == null)
            throw new KeyNotFoundException("Could not find any active orders"); 

        return orders;
    }

    public OrderDTO UpdateOrder(OrderDTO orderDTO, long id) {
        var existingOrder = _orderRepository.GetOrder(id);
        if (existingOrder == null)
            throw new KeyNotFoundException($"Order with ID {id} not found.");
        if (existingOrder.Status != OrderStatus.Open)
            throw new Exception($"Order with ID {id} is {existingOrder.Status} (not open for editing)");
        if (existingOrder.Status != orderDTO.Status)
            throw new Exception($"This endpoint is not for updating status");

        existingOrder.Tip = orderDTO.Tip;

        _orderItemUpdater.UpdateOrderItems(existingOrder, orderDTO.OrderItems);

        _orderRepository.UpdateOrder(existingOrder);

        return existingOrder.OrderToOrderDTO();
    }
}

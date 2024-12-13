using Azure.Core;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using OmgvaPOS.DiscountManagement.Mappers;
using OmgvaPOS.DiscountManagement.Models;
using OmgvaPOS.ItemManagement.Models;
using OmgvaPOS.ItemManagement.Repositories;
using OmgvaPOS.ItemManagement.Services;
using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderItemVariationManagement.Models;
using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderManagement.Enums;
using OmgvaPOS.OrderManagement.Mappers;
using OmgvaPOS.OrderManagement.Models;
using OmgvaPOS.OrderManagement.Repository;

namespace OmgvaPOS.OrderManagement.Service;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IItemService _itemService;
    private readonly IItemRepository _itemRepository;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository orderRepository,
        IItemService itemService,
        IItemRepository itemRepository,
        ILogger<OrderService> logger
        ) {
        _orderRepository = orderRepository;
        _itemService = itemService;
        _itemRepository = itemRepository;
        _logger = logger;
    }

    public OrderDTO CreateOrder(CreateOrderRequest request) {
        if (request.CreateOrderItemRequests == null || !request.CreateOrderItemRequests.Any())
            throw new ArgumentException("Order must have at least one item.");

        var order = OrderMappers.RequestToOrder(request);

        var items = order.OrderItems
            .Select(oi => _itemRepository.GetItem(oi.ItemId))
            .ToList();
        if (items.Count != request.CreateOrderItemRequests.Count)
            throw new Exception($"Some items dont exist");
        if (items.Any(item => item.IsArchived))
            throw new Exception($"Some items are archived");
        foreach (var itemReq in request.CreateOrderItemRequests) {
            if (items.Find(i => i.Id == itemReq.ItemId).InventoryQuantity < itemReq.Quantity)
                throw new Exception($"Not enough inventory for some items");
        }

        order = _orderRepository.AddOrder(order);

        foreach (var itemReq in request.CreateOrderItemRequests) {
            var item = items.Find(i => i.Id == itemReq.ItemId);
            item.InventoryQuantity -= itemReq.Quantity;
            _itemRepository.UpdateItemQuantity(item);
        }

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

    public void DeleteOrder(long id) {
        var order = _orderRepository.GetOrder(id);
        if (order.Status != OrderStatus.Open)
            throw new Exception($"Order with ID {id} is {order.Status} (not open for editing)");

        _orderRepository.DeleteOrder(order);

        var items = order.OrderItems
            .Select(oi => _itemRepository.GetItem(oi.ItemId))
            .ToList();

        foreach (var orderItem in order.OrderItems) {
            var item = items.Find(i => i.Id == orderItem.ItemId);
            item.InventoryQuantity += orderItem.Quantity;
            _itemRepository.UpdateItemQuantity(item);
        }
    }

    public void DeleteOrderItem(long orderId, long orderItemId) {
        var order = _orderRepository.GetOrder(orderId);
        if (order.Status != OrderStatus.Open)
            throw new Exception($"Order with ID {orderId} is {order.Status} (not open for editing)");

        var orderItem = order.OrderItems.FirstOrDefault(i => i.Id == orderItemId);
        if (orderItem == null)
            throw new Exception($"Cannot find order item by id: {orderItemId}");
        
        var item = _itemRepository.GetItem(orderItem.ItemId);
        item.InventoryQuantity += orderItem.Quantity;

        _orderRepository.DeleteOrderItem(orderItem);
        _itemRepository.UpdateItemQuantity(item);

    }

    public void AddOrderItem(long orderId, CreateOrderItemRequest request) {
        var item = _itemRepository.GetItem(request.ItemId);
        if (item == null) 
            throw new Exception($"Cannot find item to add by id: {request.ItemId}");
        if (item.IsArchived)
            throw new Exception($"Item {item.Name} is archived.");
        if (item.InventoryQuantity <= request.Quantity)
            throw new Exception($"Not enough {item.Name} left.");

        var order = _orderRepository.GetOrder(orderId);
        if (order == null)
            throw new Exception($"Cannot find order by id: {orderId}");
        if (order.Status != OrderStatus.Open)
            throw new Exception($"Order with ID {orderId} is {order.Status} (not open for editing)");

        var existingOrderItem = order.OrderItems.FirstOrDefault(oi => oi.ItemId == request.ItemId);
        if (existingOrderItem != null)
            throw new Exception($"Item {item.Name} is already in the order.");

        var newOrderItem = new OrderItem {
            ItemId = request.ItemId,
            Quantity = request.Quantity,
            OrderId = orderId,
            DiscountId = item.DiscountId, 
            OrderItemVariation = request.CreateOrderItemVariationRequest != null
            ? new OrderItemVariation {
                ItemVariationId = request.CreateOrderItemVariationRequest.ItemVariationId
            }
            : null
        };

        order.OrderItems.Add(newOrderItem);
        _orderRepository.AddOrderItem(newOrderItem);

        item.InventoryQuantity -= request.Quantity;
        _itemRepository.UpdateItemQuantity(item);
    }

    public void UpdateOrderItem(long orderItemId, UpdateOrderItemRequest request) {
        var orderItem = _orderRepository.GetOrderItem(orderItemId);
        if (orderItem == null)
            throw new Exception($"Cannot find orderItem by id: {orderItemId}");

        var item = _itemRepository.GetItem(orderItem.ItemId);
        if (item == null)
            throw new Exception($"Cannot find related Item by id: {orderItem.ItemId}");

        item.InventoryQuantity -= request.Quantity - orderItem.Quantity;
        orderItem.Quantity = request.Quantity;

        _itemRepository.UpdateItemQuantity(item);
        _orderRepository.UpdateOrderItemQuantity(orderItem);
    }

    public void UpdateOrderTip(short tip, long orderId) {
        var order = _orderRepository.GetOrder(orderId);
        if (order == null)
            throw new Exception($"Cannot find order by id: {orderId}");
        if (order.Status != OrderStatus.Open)
            throw new Exception($"Order with ID {orderId} is {order.Status} (not open for editing)");

        order.Tip = tip;
        _orderRepository.UpdateOrderTip(order);
    }
}

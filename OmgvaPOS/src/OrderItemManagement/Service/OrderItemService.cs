using OmgvaPOS.Database.Context;
using OmgvaPOS.ItemManagement.Repositories;
using OmgvaPOS.ItemManagement.Validators;
using OmgvaPOS.ItemVariationManagement.Models;
using OmgvaPOS.ItemVariationManagement.Repositories;
using OmgvaPOS.ItemVariationManagement.Validators;
using OmgvaPOS.OrderItemManagement.DTOs;
using OmgvaPOS.OrderItemManagement.Mappers;
using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderItemManagement.Repository;
using OmgvaPOS.OrderItemManagement.Validators;
using OmgvaPOS.OrderItemVariationManagement.Models;
using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderManagement.Repository;
using OmgvaPOS.OrderManagement.Service;
using OmgvaPOS.OrderManagement.Validators;

namespace OmgvaPOS.OrderItemManagement.Service;

public class OrderItemService : IOrderItemService
{
    private readonly OmgvaDbContext _context;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IItemVariationRepository _itemVariationRepository;
    private readonly ILogger<OrderService> _logger;
    public OrderItemService(
        OmgvaDbContext context,
        IOrderRepository orderRepository,
        IOrderItemRepository orderItemRepository,
        IItemRepository itemRepository,
        IItemVariationRepository itemVariationRepository,
        ILogger<OrderService> logger
        ) {
        _context = context;
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
        _itemRepository = itemRepository;
        _itemVariationRepository = itemVariationRepository;
        _logger = logger;
    }

    public OrderItemDTO AddOrderItem(long orderId, CreateOrderItemRequest request) {
        var item = _itemRepository.GetItem(request.ItemId);
        ItemValidator.Exists(item);
        ItemValidator.IsNotArchived(item);
        ItemValidator.EnoughInventoryQuantity(item, request.Quantity);
        item.InventoryQuantity -= request.Quantity;

        var order = _orderRepository.GetOrder(orderId);
        OrderValidator.Exists(order);
        OrderValidator.IsOpen(order);

        var newOrderItem = new OrderItem {
            ItemId = request.ItemId,
            Quantity = request.Quantity,
            OrderId = orderId,
            DiscountId = item.DiscountId,
            OrderItemVariations = new List<OrderItemVariation>()
        };

        List<ItemVariation> itemVariations = [];

        // if requested item has variations
        if (request.ItemVariationIds != null && request.ItemVariationIds.Count != 0) {
            
            foreach (var itemVariationId in request.ItemVariationIds) {
                var itemVariation = _itemVariationRepository.GetItemVariation(itemVariationId);
                ItemVariationValidator.Exists(itemVariation);
                ItemVariationValidator.IsNotArchived(itemVariation);
                ItemVariationValidator.EnoughInventoryQuantity(itemVariation, request.Quantity);
                itemVariation.InventoryQuantity -= request.Quantity;

                itemVariations.Add(itemVariation);

                newOrderItem.OrderItemVariations.Add(new OrderItemVariation {
                    ItemVariationId = itemVariation.Id
                });
            }
        }

        // OrderItemValidator.OrderItemNotInOrderYet(order, newOrderItem);

        // database updates:
        // this need a transaction, 
        // since at least 2 tables (in this case 3) are being updated
        // this ensures that if some db action fails down the line,
        // the previously succesfully completed actions do not stay.
        using var transaction = _context.Database.BeginTransaction();
        try {
            // Add the new order item
            var orderItem = _orderItemRepository.AddOrderItem(newOrderItem); ;

            // Update item inventory
            _itemRepository.UpdateItemInventoryQuantity(item);

            // Update item variation inventory
            foreach (var itemVariation in itemVariations) {
                _itemVariationRepository.UpdateItemVariationInventoryQuantity(itemVariation);
            }

            transaction.Commit();
            return OrderItemMapper.OrderItemToDTO(orderItem);
        }
        catch (Exception ex) {
            transaction.Rollback();

            _logger.LogError(ex, "An error occurred while adding the order item.");
            throw new ApplicationException("Error adding the order item. The operation has been rolled back.");
        }


    }

    public void DeleteOrderItem(long orderItemId) {
        var orderItem = _orderItemRepository.GetOrderItem(orderItemId);
        OrderItemValidator.Exists(orderItem);

        var order = _orderRepository.GetOrder(orderItem.OrderId);
        OrderValidator.Exists(order);
        OrderValidator.IsOpen(order);

        var item = _itemRepository.GetItem(orderItem.ItemId);
        ItemValidator.Exists(item);
        ItemValidator.IsNotArchived(item);
        item.InventoryQuantity += orderItem.Quantity;

        List<ItemVariation> itemVariations = [];
        // if order item has variations
        if (orderItem.OrderItemVariations != null && orderItem.OrderItemVariations.Count != 0) {

            foreach (var orderItemVariation in orderItem.OrderItemVariations) {
                var itemVariation = _itemVariationRepository.GetItemVariation(orderItemVariation.ItemVariationId);
                ItemVariationValidator.Exists(itemVariation);
                ItemVariationValidator.IsNotArchived(itemVariation);
                itemVariation.InventoryQuantity += orderItem.Quantity;

                itemVariations.Add(itemVariation);
            }
        }

        using var transaction = _context.Database.BeginTransaction();
        try {
            _orderItemRepository.DeleteOrderItem(orderItem);

            _itemRepository.UpdateItemInventoryQuantity(item);

            foreach (var itemVariation in itemVariations) {
                _itemVariationRepository.UpdateItemVariationInventoryQuantity(itemVariation);
            }

            transaction.Commit();
        }
        catch (Exception ex) {
            transaction.Rollback();

            _logger.LogError(ex, "An error occurred while deleting the order item.");
            throw new ApplicationException("Error deleting order item. The operation has been rolled back.");
        }
    }

    public OrderItemDTO GetOrderItem(long orderItemId) {
        var orderItem = _orderItemRepository.GetOrderItem(orderItemId);
        OrderItemValidator.Exists(orderItem);

        var order = _orderRepository.GetOrder(orderItem.OrderId);
        OrderValidator.Exists(order);
        OrderValidator.IsOpen(order);

        return OrderItemMapper.OrderItemToDTO(orderItem);
    }

    public void UpdateOrderItem(long orderItemId, UpdateOrderItemRequest request) {
        var orderItem = _orderItemRepository.GetOrderItem(orderItemId);
        OrderItemValidator.Exists(orderItem);

        var changeInQuantity = request.Quantity - orderItem.Quantity;
        orderItem.Quantity = request.Quantity;

        var order = _orderRepository.GetOrder(orderItem.OrderId);
        OrderValidator.Exists(order);
        OrderValidator.IsOpen(order);

        var item = _itemRepository.GetItem(orderItem.ItemId);
        ItemValidator.Exists(item);
        ItemValidator.IsNotArchived(item);
        ItemValidator.EnoughInventoryQuantity(item, changeInQuantity);
        item.InventoryQuantity -= changeInQuantity;

        List<ItemVariation> itemVariations = [];
        // if order item has variations
        if (orderItem.OrderItemVariations != null && orderItem.OrderItemVariations.Count != 0) {

            foreach (var orderItemVariation in orderItem.OrderItemVariations) {
                var itemVariation = _itemVariationRepository.GetItemVariation(orderItemVariation.ItemVariationId);
                ItemVariationValidator.Exists(itemVariation);
                ItemVariationValidator.IsNotArchived(itemVariation);
                ItemVariationValidator.EnoughInventoryQuantity(itemVariation, changeInQuantity);
                itemVariation.InventoryQuantity -= changeInQuantity;

                itemVariations.Add(itemVariation);
            }
        }

        using var transaction = _context.Database.BeginTransaction();
        try {
            _orderItemRepository.UpdateOrderItemQuantity(orderItem); ;

            _itemRepository.UpdateItemInventoryQuantity(item);

            foreach (var itemVariation in itemVariations) {
                _itemVariationRepository.UpdateItemVariationInventoryQuantity(itemVariation);
            }

            transaction.Commit();
        }
        catch (Exception ex) {
            transaction.Rollback();

            _logger.LogError(ex, "An error occurred while updating the order item.");
            throw new ApplicationException("Error updating order item. The operation has been rolled back.");
        }
    }
}

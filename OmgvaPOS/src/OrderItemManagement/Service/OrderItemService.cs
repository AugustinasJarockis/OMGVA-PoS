using OmgvaPOS.Database.Context;
using OmgvaPOS.ItemManagement.Repositories;
using OmgvaPOS.ItemManagement.Services;
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
using OmgvaPOS.OrderManagement.Mappers;
using OmgvaPOS.OrderManagement.Repository;
using OmgvaPOS.OrderManagement.Service;
using OmgvaPOS.OrderManagement.Validators;
using OmgvaPOS.TaxManagement.Models;

namespace OmgvaPOS.OrderItemManagement.Service;

public class OrderItemService : IOrderItemService
{
    private readonly OmgvaDbContext _context;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IItemService _itemService;
    private readonly IItemRepository _itemRepository;
    private readonly IItemVariationRepository _itemVariationRepository;
    private readonly ILogger<OrderService> _logger;

    public OrderItemService(
        OmgvaDbContext context,
        IOrderRepository orderRepository,
        IOrderItemRepository orderItemRepository,
        IItemService itemService,
        IItemRepository itemRepository,
        IItemVariationRepository itemVariationRepository,
        ILogger<OrderService> logger
        ) {
        _context = context;
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
        _itemService = itemService;
        _itemRepository = itemRepository;
        _itemVariationRepository = itemVariationRepository;
        _logger = logger;
    }

    public void AddOrderItem(long orderId, CreateOrderItemRequest request) {
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
            _orderItemRepository.AddOrderItem(newOrderItem); ;

            // Update item inventory
            _itemRepository.UpdateItemInventoryQuantity(item);

            // Update item variation inventory
            foreach (var itemVariation in itemVariations) {
                _itemVariationRepository.UpdateItemVariationInventoryQuantity(itemVariation);
            }

            transaction.Commit();
        }
        catch (Exception ex) {
            transaction.Rollback();

            _logger.LogError(ex, "An error occurred while adding the order item.");
            throw new ApplicationException("Error adding the order item. The operation has been rolled back.");
        }
    }

    public void DeleteOrderItem(long orderItemId, bool useTransaction) {
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

        // DeleteOrderItem may be called inside a transaction, 
        // in that case, using a nested transaction is not only unnecesary,
        // but also causes major errors
        if (useTransaction) {
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
        else {
            _orderItemRepository.DeleteOrderItem(orderItem);

            _itemRepository.UpdateItemInventoryQuantity(item);

            foreach (var itemVariation in itemVariations) {
                _itemVariationRepository.UpdateItemVariationInventoryQuantity(itemVariation);
            }
        }
        
    }

    // only callable from OrderService
    public OrderItemDTO GetOrderItem(long orderItemId) {
        // get OrderItem itself
        var orderItem = _orderItemRepository.GetOrderItemOrThrow(orderItemId);

        // Get Item for name, prices and taxes.
        var item = _itemService.GetItemOrThrow(orderItem.ItemId);

        // Get OrderItem discount for
        // calculating OrderItem total price
        // and including in OrderItemDTO 
        var orderItemDiscountDTO = orderItem.Discount.ToSimpleDiscountDTO();

        // Get OrderItemVariations for
        // calculating OrderItem total price 
        // and including in OrderItemDTO
        List<OrderItemVariationDTO> orderItemVariationDTOs = [];
        foreach (var orderItemVariation in orderItem.OrderItemVariations) {
            var itemVariation = _itemVariationRepository.GetItemVariation(orderItemVariation.ItemVariationId);
            ItemVariationValidator.Exists(itemVariation);

            orderItemVariationDTOs.Add(new OrderItemVariationDTO {
                Id = orderItemVariation.Id,
                ItemVariationId = orderItemVariation.ItemVariationId,
                ItemVariationName = itemVariation.Name,
                ItemVariationGroup = itemVariation.ItemVariationGroup,
                PriceChange = itemVariation.PriceChange
            });
        }

        // OrderItemPriceCalculation
        decimal defaultUnitPrice = item.Price;
        decimal unitPriceWithVariations = defaultUnitPrice + orderItemVariationDTOs.Sum(oIV => oIV.PriceChange);
        decimal unitPriceWithDiscounts = unitPriceWithVariations - (orderItemDiscountDTO?.DiscountAmount ?? 0);
        decimal totalTaxPercent = item.TaxItems?.Select(ti => ti.Tax).Sum(iT => iT.Percent) ?? 0;
        decimal unitPriceWithTaxes = unitPriceWithDiscounts * (100 + totalTaxPercent) / 100;
        decimal totalPrice = unitPriceWithTaxes * orderItem.Quantity;

        OrderItemDTO orderItemDTO = new OrderItemDTO {
            Id = orderItem.Id,
            TotalPrice = totalPrice,
            UnitPrice = unitPriceWithTaxes,
            ItemId = orderItem.Id,
            ItemName = item.Name,
            Quantity = orderItem.Quantity,
            Discount = orderItemDiscountDTO,
            Variations = orderItemVariationDTOs
        };

        return orderItemDTO;
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

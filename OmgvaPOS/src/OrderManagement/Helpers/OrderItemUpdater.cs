using OmgvaPOS.ItemManagement.Repositories;
using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderItemVariationManagement.Models;
using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderManagement.Models;
using OmgvaPOS.OrderManagement.Repository;

namespace OmgvaPOS.OrderManagement.Helpers;

public class OrderItemUpdater
{
    private readonly IOrderRepository _orderRepository;
    private readonly IItemRepository _itemRepository;

    // TODO: Add all kinds of error checking on update.
    public OrderItemUpdater(
        IOrderRepository orderRepository,
        IItemRepository itemRepository
        ) {
        _orderRepository = orderRepository;
        _itemRepository = itemRepository;
    }

    public void UpdateOrderItems(Order existingOrder, ICollection<OrderItemDTO> updatedOrderItems) {
        // Remove items not in the updated list
        foreach (var existingItem in existingOrder.OrderItems.ToList()) {
            if (!updatedOrderItems.Any(u => u.Id == existingItem.Id)) {
                _orderRepository.RemoveOrderItem( existingItem );
            }
        }

        // Update or add items
        foreach (var updatedItem in updatedOrderItems) {
            var existingItem = existingOrder.OrderItems.FirstOrDefault(e => e.Id == updatedItem.Id);
            if (existingItem == null) { // Add new item
                AddItem(existingOrder, updatedItem);
            }
            else {
                UpdateItem(existingItem, updatedItem);
            }
        }
    }

    private void AddItem(Order existingOrder, OrderItemDTO updatedItem) {
        var item = _itemRepository.GetItem(updatedItem.ItemId);
        if (item == null)
            throw new KeyNotFoundException("The item that you are trying to add to an order doesnt exist.");
        
        var newOrderItem = new OrderItem {
            ItemId = updatedItem.ItemId,
            Quantity = updatedItem.Quantity,
            DiscountId = item.DiscountId,
            OrderItemVariation = updatedItem.OrderItemVariation == null
                        ? null
                        : new OrderItemVariation {
                            ItemVariationId = updatedItem.OrderItemVariation.ItemVariationId
                        }
        };
        existingOrder.OrderItems.Add(newOrderItem);
    }

    private void UpdateItem(OrderItem existingItem, OrderItemDTO updatedItem) {
        // Update existing item
        existingItem.Quantity = updatedItem.Quantity;
        existingItem.DiscountId = updatedItem.DiscountId;

        // Update OrderItemVariation
        if (updatedItem.OrderItemVariation != null) {
            if (existingItem.OrderItemVariation == null) {
                existingItem.OrderItemVariation = new OrderItemVariation {
                    ItemVariationId = updatedItem.OrderItemVariation.ItemVariationId
                };
            }
            else {
                existingItem.OrderItemVariation.ItemVariationId = updatedItem.OrderItemVariation.ItemVariationId;
            }
        }
        else {
            existingItem.OrderItemVariation = null; // Remove variation if not present in DTO
        }
    }
}

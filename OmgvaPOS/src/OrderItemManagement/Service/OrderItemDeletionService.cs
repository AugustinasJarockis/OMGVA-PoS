using OmgvaPOS.Database.Context;
using OmgvaPOS.ItemManagement.Repositories;
using OmgvaPOS.ItemManagement.Validators;
using OmgvaPOS.ItemVariationManagement.Models;
using OmgvaPOS.ItemVariationManagement.Repositories;
using OmgvaPOS.ItemVariationManagement.Validators;
using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderItemManagement.Repository;
using OmgvaPOS.OrderItemManagement.Validators;
using OmgvaPOS.OrderManagement.Repository;
using OmgvaPOS.OrderManagement.Validators;

namespace OmgvaPOS.OrderItemManagement.Service
{
    public class OrderItemDeletionService: IOrderItemDeletionService
    {
        private readonly OmgvaDbContext _context;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IItemVariationRepository _itemVariationRepository;
        private readonly ILogger<OrderItemDeletionService> _logger;
        public OrderItemDeletionService(
            OmgvaDbContext context,
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            IItemRepository itemRepository,
            IItemVariationRepository itemVariationRepository,
            ILogger<OrderItemDeletionService> logger
            ) {
            _context = context;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _itemRepository = itemRepository;
            _itemVariationRepository = itemVariationRepository;
            _logger = logger;
        }
        public void DeleteOrderItem(long orderItemId, bool useTransaction = false) {
            var orderItem = _orderItemRepository.GetOrderItem(orderItemId);
            OrderItemValidator.Exists(orderItem);

            var order = _orderRepository.GetOrder(orderItem.OrderId);
            OrderValidator.Exists(order);
            OrderValidator.IsOpen(order);

            // DeleteOrderItem may be called inside a transaction, 
            // in that case, using a nested transaction is not only unnecesary,
            // but also causes major errors
            if (useTransaction) {
                using var transaction = _context.Database.BeginTransaction();
                try {
                    ReturnItemsToInventory([orderItem]);

                    _orderItemRepository.DeleteOrderItem(orderItem);

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

                ReturnItemsToInventory([orderItem]);
            }

        }

        public void ReturnItemsToInventory(ICollection<OrderItem> orderItems) {
            foreach (var orderItem in orderItems) {
                var item = _itemRepository.GetItem(orderItem.Id);
                if (item.Duration != null) break; // check if its a service, if yes -> do nothing with item

                item.InventoryQuantity += orderItem.Quantity;
                _itemRepository.UpdateItemInventoryQuantity(item);

                var orderItemWithVariations = _orderItemRepository.GetOrderItem(orderItem.Id);
                if (orderItemWithVariations.OrderItemVariations != null && orderItemWithVariations.OrderItemVariations.Count != 0) {
                    foreach (var orderItemVariation in orderItemWithVariations.OrderItemVariations) {
                        var itemVariation = _itemVariationRepository.GetItemVariation(orderItemVariation.ItemVariationId);
                        ItemVariationValidator.Exists(itemVariation);
                        itemVariation.InventoryQuantity += orderItem.Quantity;
                        _itemVariationRepository.UpdateItemVariationInventoryQuantity(itemVariation);
                    }
                }
            }
        }
    }
}

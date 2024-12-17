using OmgvaPOS.Database.Context;
using OmgvaPOS.ItemManagement.Repositories;
using OmgvaPOS.ItemManagement.Validators;
using OmgvaPOS.ItemVariationManagement.Models;
using OmgvaPOS.ItemVariationManagement.Repositories;
using OmgvaPOS.ItemVariationManagement.Validators;
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

            var item = _itemRepository.GetItem(orderItem.ItemId);
            ItemValidator.Exists(item);
            item.InventoryQuantity += orderItem.Quantity;

            List<ItemVariation> itemVariations = [];
            // if order item has variations
            if (orderItem.OrderItemVariations != null && orderItem.OrderItemVariations.Count != 0) {

                foreach (var orderItemVariation in orderItem.OrderItemVariations) {
                    var itemVariation = _itemVariationRepository.GetItemVariation(orderItemVariation.ItemVariationId);
                    ItemVariationValidator.Exists(itemVariation);
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
    }
}

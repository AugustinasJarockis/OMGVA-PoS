using OmgvaPOS.Exceptions;
using OmgvaPOS.ItemManagement.Services;
using OmgvaPOS.ItemVariationManagement.DTOs;
using OmgvaPOS.ItemVariationManagement.Mappers;
using OmgvaPOS.ItemVariationManagement.Models;
using OmgvaPOS.ItemVariationManagement.Repositories;
using OmgvaPOS.OrderItemManagement.Repository;
using OmgvaPOS.OrderItemManagement.Service;
using OmgvaPOS.OrderManagement.Repository;
using OmgvaPOS.OrderManagement.Service;

namespace OmgvaPOS.ItemVariationManagement.Services
{
    public class ItemVariationService(
        IItemVariationRepository itemVariationRepository,
        IItemService itemService,
        IOrderItemRepository orderItemRepository,
        IOrderItemDeletionService orderItemDeletionService,
        ILogger<ItemVariationService> logger
        ) : IItemVariationService
    {
        private readonly IItemVariationRepository _itemVariationRepository = itemVariationRepository;
        private readonly IItemService _itemService = itemService;
        private readonly IOrderItemRepository _orderItemRepository = orderItemRepository;
        private readonly IOrderItemDeletionService _orderItemDeletionService = orderItemDeletionService;
        private readonly ILogger<ItemVariationService> _logger = logger;

        public List<ItemVariationDTO> GetItemVariations(long itemId) {
            var itemVariations = _itemVariationRepository.GetItemVariations(itemId);
            var itemVariationDTOs = itemVariations.Select(i => i.ToItemVariationDTO()).ToList();
            return itemVariationDTOs;
        }
        public ItemVariationDTO? GetItemVariation(long id) {
            var itemVariation = _itemVariationRepository.GetItemVariation(id);
            return itemVariation?.ToItemVariationDTO();
        }

        public long GetItemVariationBusinessId(long id)
        {
            var itemVariation = GetItemVariationOrThrow(id);

            return _itemService.GetItemBusinessId(itemVariation.ItemId);
        }
        public ItemVariationDTO CreateItemVariation(ItemVariationCreationRequest itemVariationCreationRequest, long itemId) {
            ItemVariation itemVariation = itemVariationCreationRequest.ToItemVariation();
            itemVariation.ItemId = itemId;
            var newItemVariation = _itemVariationRepository.CreateItemVariation(itemVariation);
            return newItemVariation.ToItemVariationDTO();
        }
        public ItemVariationDTO UpdateItemVariation(ItemVariationUpdateRequest itemVariationUpdateRequest, long id)
        {
            var itemVariation = GetItemVariationOrThrow(id);

            var relatedOrderItems = _orderItemRepository.GetOrderItemsByItemId(itemVariation.ItemId);
            foreach (var orderItem in relatedOrderItems) {
                _orderItemDeletionService.DeleteOrderItem(orderItem.Id, true);
            }
            
            itemVariation = itemVariationUpdateRequest.ToItemVariation(itemVariation);
            return _itemVariationRepository.UpdateItemVariation(itemVariation).ToItemVariationDTO();
        }
        public void DeleteItemVariation(long id) {
            var itemVariation = GetItemVariationOrThrow(id);

            var relatedOrderItems = _orderItemRepository.GetOrderItemsByItemId(itemVariation.ItemId);
            foreach (var orderItem in relatedOrderItems) {
                _orderItemDeletionService.DeleteOrderItem(orderItem.Id, true);
            }

            _itemVariationRepository.DeleteItemVariation(id);
        }

        private ItemVariation GetItemVariationOrThrow(long itemVariationId)
        {
            var itemVariation = _itemVariationRepository.GetItemVariation(itemVariationId);
            if (itemVariation == null)
                throw new NotFoundException("Item variation not found");

            return itemVariation;
        }
        
    }
}

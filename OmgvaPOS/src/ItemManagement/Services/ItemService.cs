using OmgvaPOS.DiscountManagement.Service;
using OmgvaPOS.Exceptions;
using OmgvaPOS.TaxManagement.Repository;
using OmgvaPOS.ItemManagement.Repositories;
using OmgvaPOS.ItemVariationManagement.Repositories;
using OmgvaPOS.ItemManagement.Models;
using OmgvaPOS.ItemManagement.DTOs;
using OmgvaPOS.ItemManagement.Mappers;
using OmgvaPOS.ItemManagement.Validators;
using OmgvaPOS.TaxManagement.Models;
using OmgvaPOS.TaxManagement.Mappers;
using OmgvaPOS.UserManagement.Service;
using OmgvaPOS.OrderItemManagement.Repository;
using OmgvaPOS.OrderItemManagement.Service;

namespace OmgvaPOS.ItemManagement.Services
{
    public class ItemService(
        IItemRepository itemRepository,
        IItemVariationRepository itemVariationRepository,
        ITaxRepository taxRepository,
        ITaxItemRepository taxItemRepository,
        IOrderItemDeletionService orderItemDeletionService,
        IOrderItemRepository orderItemRepository,
        IUserService userService,
        DiscountValidatorService discountValidatorService,
        ILogger<ItemService> logger
        ) : IItemService
    {
        private readonly ITaxRepository _taxRepository = taxRepository;
        private readonly ITaxItemRepository _taxItemRepository = taxItemRepository;
        private readonly IItemRepository _itemRepository = itemRepository;
        private readonly IItemVariationRepository _itemVariationRepository = itemVariationRepository;
        private readonly IOrderItemDeletionService _orderItemDeletionService = orderItemDeletionService;
        private readonly IOrderItemRepository _orderItemRepository = orderItemRepository;
        private readonly IUserService _userService = userService;
        private readonly DiscountValidatorService _discountValidatorService = discountValidatorService;
        private readonly ILogger<ItemService> _logger = logger;

        public List<ItemDTO> GetItems(long businessId) {
            var items = _itemRepository.GetItems(businessId);
            var itemDTOs = items.Select(i => i.ToItemDTO()).ToList();
            return itemDTOs;
        }
        public ItemDTO? GetItem(long id) {
            var item = _itemRepository.GetItem(id);
            return item?.ToItemDTO();
        }

        public Item GetItemNoException(long id) {
            return _itemRepository.GetItem(id);
        }

        public Item GetItemOrThrow(long itemId)
        {
            var item = _itemRepository.GetItem(itemId);
            if (item == null)
                throw new NotFoundException("Item not found");

            return item;
        }

        public long GetItemBusinessId(long itemId)
        {
            return GetItemOrThrow(itemId).BusinessId;
        }
        
        public ItemDTO CreateItem(CreateItemRequest createItemRequest, long businessId) {
            createItemRequest.Currency = createItemRequest.Currency.ToUpper();
            ItemValidator.ValidateCreateItemRequest(createItemRequest);
            
            _userService.ValidateUserBelongsToBusiness(createItemRequest.UserId, businessId);
            _discountValidatorService.ValidateDiscountBelongsToBusiness(createItemRequest.DiscountId, businessId);
            
            var newItemData = createItemRequest.ToItem(businessId);
            var newItem = _itemRepository.CreateItem(newItemData);
            
            return newItem.ToItemDTO();
        }
        
        public ItemDTO UpdateItem(ItemDTO updateItemRequest, long itemId)
        {
            var currentItem = GetItemOrThrow(itemId);
            ItemValidator.IsNotArchived(currentItem);
            
            _userService.ValidateUserBelongsToBusiness(updateItemRequest.UserId, currentItem.BusinessId);
            _discountValidatorService.ValidateDiscountBelongsToBusiness(updateItemRequest.DiscountId, currentItem.BusinessId);
            
            currentItem = updateItemRequest.ToItem(currentItem);
            return UpdateItemInternal(currentItem).ToItemDTO();
        }

        // Duplicate current item with same item variations and taxItems
        // Archive the previous item
        private Item UpdateItemInternal(Item currentItem) {
            ItemValidator.IsNotArchived(currentItem);
            var itemVariations = _itemVariationRepository.GetItemVariationQueriable().Where(v => v.ItemId == currentItem.Id);
            var taxItemQueriable = _taxItemRepository.GetAllTaxItemQueriable().Where(t => t.ItemId == currentItem.Id);

            var relatedOrderItems = _orderItemRepository.GetOrderItemsByItemId(currentItem.Id);
            foreach (var orderItem in relatedOrderItems) {
                _orderItemDeletionService.DeleteOrderItem(orderItem.Id, true);
            }

            var recreatedItem = _itemRepository.UpdateItem(currentItem);
            _taxItemRepository.CreateConnectionsForNewItem(taxItemQueriable, recreatedItem.Id);
            _itemVariationRepository.DuplicateItemVariations(itemVariations, recreatedItem.Id);
            
            return recreatedItem;
        }

        public void DuplicateItems(IEnumerable<Item> items) {
            var itemList = items.ToList();
            foreach (Item item in itemList) {
                var itemVariations = _itemVariationRepository.GetItemVariationQueriable().Where(v => v.ItemId == item.Id);
                var taxItemQueriable = _taxItemRepository.GetAllTaxItemQueriable().Where(t => t.ItemId == item.Id);

                var recreatedItem = _itemRepository.UpdateItem(item);
                _taxItemRepository.CreateConnectionsForNewItem(taxItemQueriable, recreatedItem.Id);
                _itemVariationRepository.DuplicateItemVariations(itemVariations, recreatedItem.Id);
            }
        }
        public void DeleteItem(long id)
        {
            if (_itemRepository.GetItem(id) == null)
                throw new NotFoundException();

            var relatedOrderItems = _orderItemRepository.GetOrderItemsByItemId(id);
            foreach (var orderItem in relatedOrderItems) {
                _orderItemDeletionService.DeleteOrderItem(orderItem.Id, true);
            }

            var itemVariations = _itemVariationRepository.GetItemVariationQueriable().Where(v => v.ItemId == id);
            _itemVariationRepository.DeleteItemVariations(itemVariations.Select(v => v.Id));
            _itemRepository.DeleteItem(id);
        }

        public List<TaxDto> GetItemTaxes(long id) {
            var taxItems = _taxItemRepository.GetAllTaxItemQueriable().Where(c => c.ItemId == id);
            return _taxRepository.GetAllTaxes()
                .Where(t => taxItems.Select(c => c.TaxId).Contains(t.Id))
                .Select(TaxMapper.ToDTO).ToList();
        }
        public ItemDTO ChangeItemTaxes(ChangeItemTaxesRequest changeItemTaxesRequest, long itemId)
        {
            var currentItem = GetItemOrThrow(itemId);
            ItemValidator.IsNotArchived(currentItem);
            
            var newItem = UpdateItemInternal(currentItem);
            var taxItems = _taxItemRepository.GetAllTaxItemQueriable();
            
            var taxItemIdsToRemove = taxItems
                .Where(c => c.ItemId == newItem.Id && changeItemTaxesRequest.TaxesToRemoveIds.Contains(c.TaxId))
                .Select(c => c.Id);
            _taxItemRepository.DeleteTaxItems(taxItemIdsToRemove);
            _taxItemRepository.AddConnectionsBetweenItemAndTaxes(changeItemTaxesRequest.TaxesToAddIds, newItem.Id);

            return newItem.ToItemDTO();
        }
    }
}

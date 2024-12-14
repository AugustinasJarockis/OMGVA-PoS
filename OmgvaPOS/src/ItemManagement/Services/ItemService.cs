using OmgvaPOS.TaxManagement.Repository;
using OmgvaPOS.ItemManagement.Repositories;
using OmgvaPOS.ItemVariationManagement.Repositories;
using OmgvaPOS.ItemManagement.Models;
using OmgvaPOS.ItemManagement.DTOs;
using OmgvaPOS.ItemManagement.Mappers;
using OmgvaPOS.ItemManagement.Validator;
using OmgvaPOS.TaxManagement.Models;
using OmgvaPOS.TaxManagement.Mappers;

namespace OmgvaPOS.ItemManagement.Services
{
    public class ItemService(
        IItemRepository itemRepository,
        IItemVariationRepository itemVariationRepository,
        ITaxRepository taxRepository,
        ITaxItemRepository taxItemRepository,
        ILogger<ItemService> logger
        ) : IItemService
    {
        private readonly ITaxRepository _taxRepository = taxRepository;
        private readonly ITaxItemRepository _taxItemRepository = taxItemRepository;
        private readonly IItemRepository _itemRepository = itemRepository;
        private readonly IItemVariationRepository _itemVariationRepository = itemVariationRepository;
        private readonly ILogger<ItemService> _logger = logger;

        //TODO: Think how you will add taxes, discounts
        public List<ItemDTO> GetItems(long businessId) {
            var items = _itemRepository.GetItems(businessId);
            var itemDTOs = items.Select(i => i.ToItemDTO()).ToList();
            return itemDTOs;
        }
        public ItemDTO GetItem(long id) {
            var item = _itemRepository.GetItem(id);
            return item.ToItemDTO();
        }

        public Item GetItemNoException(long id) {
            return _itemRepository.GetItem(id);
        }
        public ItemDTO CreateItem(CreateItemRequest createItemRequest, long businessId) {
            //TODO: Think about discounts
            createItemRequest.Currency = createItemRequest.Currency.ToUpper();
            ItemValidator.ValidateCreateItemRequest(createItemRequest);
            
            var newItemData = createItemRequest.ToItem(businessId);
            var newItem = _itemRepository.CreateItem(newItemData);
            
            return newItem.ToItemDTO();
        }
        public ItemDTO? UpdateItem(ItemDTO updateItemRequest) {
            var item = _itemRepository.GetItem((long)updateItemRequest.Id); //TODO: potential error here. Though unlikely as endpoint should make sure of id existance
            item = updateItemRequest.FromUpdateRequestToItem(item);
            return UpdateItem(item).ToItemDTO();
        }

        private Item UpdateItem(Item item) {
            var itemVariations = _itemVariationRepository.GetItemVariationQueriable().Where(v => v.ItemId == item.Id);
            var taxItemQueriable = _taxItemRepository.GetAllTaxItemQueriable().Where(t => t.ItemId == item.Id);

            var recreatedItem = _itemRepository.UpdateItem(item);
            _taxItemRepository.CreateConnectionsForNewItem(taxItemQueriable, recreatedItem.Id);
            _itemVariationRepository.DuplicateItemVariations(itemVariations, recreatedItem.Id);
            //TODO: Update open orders
            //TODO: Think about discounts
            return recreatedItem;
        }

        public void DuplicateItems(IEnumerable<Item> items) {
            foreach (Item item in items) {
                var itemVariations = _itemVariationRepository.GetItemVariationQueriable().Where(v => v.ItemId == item.Id);
                var taxItemQueriable = _taxItemRepository.GetAllTaxItemQueriable().Where(t => t.ItemId == item.Id);

                var recreatedItem = _itemRepository.UpdateItem(item);
                _taxItemRepository.CreateConnectionsForNewItem(taxItemQueriable, recreatedItem.Id);
                _itemVariationRepository.DuplicateItemVariations(itemVariations, recreatedItem.Id);
            }
        }
        public void DeleteItem(long id) {
            var itemVariations = _itemVariationRepository.GetItemVariationQueriable().Where(v => v.ItemId == id);
            _itemVariationRepository.DeleteItemVariations(itemVariations.Select(v => v.Id));
            _itemRepository.DeleteItem(id);

            //TODO: Delete from open orders?
            //TODO: Think about discounts
        }

        public List<TaxDto> GetItemTaxes(long id) {
            var taxItems = _taxItemRepository.GetAllTaxItemQueriable().Where(c => c.ItemId == id);
            return _taxRepository.GetAllTaxes()
                .Where(t => taxItems.Select(c => c.TaxId).Contains(t.Id))
                .Select(t => TaxMapper.ToDTO(t)).ToList();
        }
        public ItemDTO ChangeItemTaxes(ChangeItemTaxesRequest changeItemTaxesRequest, long itemId) {
            // TODO: REWRITE for better clarity. 
            var newItem = UpdateItem(_itemRepository.GetItem(itemId)); //TODO: Potential error here

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

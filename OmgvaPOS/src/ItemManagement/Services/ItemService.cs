using OmgvaPOS.TaxManagement.Repository;
using OmgvaPOS.ItemManagement.Repositories;
using OmgvaPOS.ItemVariationManagement.Repositories;
using OmgvaPOS.ItemManagement.Models;
using OmgvaPOS.ItemManagement.DTOs;
using OmgvaPOS.ItemManagement.Mappers;

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
            try {
                return _itemRepository.GetItem(id);
            }
            catch (Exception ex) {
                _logger.LogError("Unexpected error occurred when trying to get item: " + ex);
                return null;
            }
        }
        public ItemDTO CreateItem(Item item) {
            //TODO: Think about discounts
            var newItem = _itemRepository.CreateItem(item);
            return newItem.ToItemDTO();
        }
        public ItemDTO UpdateItem(ItemDTO itemDTO) {
            var item = _itemRepository.GetItem((long)itemDTO.Id); //TODO: potential error here. Though unlikely as endpoint should make sure of id existance
            item = itemDTO.ToItem(item);
            var itemVariations = _itemVariationRepository.GetItemVariationQueriable().Where(v => v.ItemId == item.Id);
            var taxItemQueriable = _taxItemRepository.GetAllTaxItemQueriable().Where(t => t.ItemId == item.Id);

            var recreatedItem = _itemRepository.UpdateItem(item);
            _taxItemRepository.CreateConnectionsForNewItem(taxItemQueriable, recreatedItem.Id);
            _itemVariationRepository.DuplicateItemVariations(itemVariations, recreatedItem.Id);
            //TODO: Update open orders
            //TODO: Think about discounts
            return recreatedItem.ToItemDTO();
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
    }
}

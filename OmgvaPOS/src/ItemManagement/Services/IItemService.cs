using OmgvaPOS.ItemManagement.DTOs;
using OmgvaPOS.ItemManagement.Models;
using OmgvaPOS.TaxManagement.Models;

namespace OmgvaPOS.ItemManagement.Services
{
    public interface IItemService
    {
        public List<ItemDTO> GetItems(long businessId);
        public ItemDTO? GetItem(long id);
        public Item GetItemOrThrow(long itemId);
        public Item GetItemNoException(long id);
        public ItemDTO CreateItem(CreateItemRequest createItemRequest, long businessId);
        public void DuplicateItems(IEnumerable<Item> items);
        public ItemDTO UpdateItem(ItemDTO item, long itemId);
        public void DeleteItem(long id);

        public List<TaxDto> GetItemTaxes(long id);
        public ItemDTO ChangeItemTaxes(ChangeItemTaxesRequest changeItemTaxesRequest, long itemId);
        public long GetItemBusinessId(long itemId);
    }
}

using OmgvaPOS.ItemManagement.DTOs;
using OmgvaPOS.ItemManagement.Models;

namespace OmgvaPOS.ItemManagement.Services
{
    public interface IItemService
    {
        public List<ItemDTO> GetItems(long businessId);
        public ItemDTO GetItem(long id);
        public Item GetItemNoException(long id);
        public ItemDTO CreateItem(Item item);
        public void DuplicateItems(IEnumerable<Item> items);
        public ItemDTO UpdateItem(ItemDTO item);
        public void DeleteItem(long id);
    }
}

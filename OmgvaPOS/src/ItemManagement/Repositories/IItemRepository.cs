using OmgvaPOS.ItemManagement.Models;

namespace OmgvaPOS.ItemManagement.Repositories
{
    public interface IItemRepository
    {
        public IQueryable<Item> GetItemsQueriable();
        public List<Item> GetItems(long businessId);

        public Item GetItem(long id);

        public Item CreateItem(Item item);

        public Item UpdateItem(Item item);

        public void DeleteItem(long id);
        public void UpdateItemQuantity(Item item);
    }
}

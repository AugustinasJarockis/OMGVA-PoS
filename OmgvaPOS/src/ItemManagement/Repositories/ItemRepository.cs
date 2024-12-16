using OmgvaPOS.Database.Context;
using OmgvaPOS.ItemManagement.Models;
using OmgvaPOS.ItemManagement.Validators;
using OmgvaPOS.ItemVariationManagement.Repositories;
using OmgvaPOS.TaxManagement.Models;
using OmgvaPOS.TaxManagement.Repository;

namespace OmgvaPOS.ItemManagement.Repositories
{
    public class ItemRepository(OmgvaDbContext database, ILogger<ItemRepository> logger) : IItemRepository
    {
        private readonly OmgvaDbContext _database = database;
        private readonly ILogger<ItemRepository> _logger = logger;
        public IQueryable<Item> GetItemsQueriable() {
            return _database.Items;
        }

        public List<Item> GetItems(long businessId) {
            return _database.Items
                .Where(t => t.IsArchived == false && t.BusinessId == businessId).ToList();
        }

        public Item? GetItem(long id) {
            return _database.Items.Where(item => item.Id == id).FirstOrDefault();
        }

        public Item CreateItem(Item item) {
            _database.Items.Add(item);
            _database.SaveChanges();
            return item;
        }

        public Item UpdateItem(Item item) // TODO: Proper error handling
        {
            var newItem = (Item)item.Clone();
            _database.Add(newItem);
            _database.Entry(item).CurrentValues.SetValues(_database.Entry(item).OriginalValues);
            item.IsArchived = true;
            _database.Items.Update(item);
            _database.SaveChanges();
            return newItem;
        }

        public void UpdateItemInventoryQuantity(Item item) {
            try {
                _database.Items.Update(item);
                _database.SaveChanges();
            }
            catch (Exception ex) {
                _logger.LogError(ex, $"An error occurred while updating item with ID {item.Id}.");
                throw new ApplicationException("Error updating item.");
            }
        }

        public void DeleteItem(long id) {
            var item = _database.Items.Find(id);
            if (item != null) {
                item.IsArchived = true;
                _database.Items.Update(item);
                _database.SaveChanges();
            }
        }
    }
}

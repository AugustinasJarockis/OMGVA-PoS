using OmgvaPOS.Database.Context;
using OmgvaPOS.ItemManagement.Models;
using OmgvaPOS.ItemVariationManagement.Repositories;
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
            try {
                return _database.Items
                    .Where(t => t.IsArchived == false && t.BusinessId == businessId).ToList();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred while retrieving all items.");
                throw new ApplicationException("Error retrieving items.");
            }
        }

        public Item GetItem(long id) {
            try {
                return _database.Items.Where(item => item.Id == id).FirstOrDefault();
            }
            catch (Exception ex) {
                _logger.LogError(ex, $"An error occurred while retrieving item with ID {id}.");
                throw new ApplicationException("Error retrieving item.");
            }
        }

        public Item CreateItem(Item item) {
            try {
                _database.Items.Add(item);
                _database.SaveChanges();
                return item;
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred while creating item.");
                throw new ApplicationException("An unexpected error occurred while creating item.");
            }
        }

        public Item UpdateItem(Item item) //TODO: Check if this is actually correct // TODO: Proper error handling
        {
            try {
                var oldItem = _database.Items.Find(item.Id);
                _database.Add(item);
                oldItem.IsArchived = true;
                _database.Items.Update(oldItem);
                _database.SaveChanges();
                return item;
            }
            catch (Exception ex) {
                _logger.LogError(ex, $"An error occurred while updating item with ID {item.Id}.");
                throw new ApplicationException("Error updating item.");
            }
        }

        public void DeleteItem(long id) {
            try {
                var item = _database.Items.Find(id);
                if (item != null) {
                    item.IsArchived = true;
                    _database.Items.Update(item);
                    _database.SaveChanges();
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, $"An error occurred while deleting an item with ID {id}.");
                throw new ApplicationException("Error deleting item.");
            }
        }
    }
}

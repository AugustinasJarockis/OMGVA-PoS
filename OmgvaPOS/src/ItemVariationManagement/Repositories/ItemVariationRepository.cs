using Microsoft.EntityFrameworkCore;
using OmgvaPOS.Database.Context;
using OmgvaPOS.ItemVariationManagement.Models;

namespace OmgvaPOS.ItemVariationManagement.Repositories
{
    public class ItemVariationRepository(OmgvaDbContext database, ILogger<ItemVariationRepository> logger): IItemVariationRepository
    {
        private readonly OmgvaDbContext _database = database;
        private readonly ILogger<ItemVariationRepository> _logger = logger;

        public List<ItemVariation> GetItemVariations(long itemId) {
            try {
                return _database.ItemVariations
                    .Where(v => v.IsArchived == false && v.ItemId == itemId).ToList();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred while retrieving item's variations.");
                throw new ApplicationException("Error retrieving item variations.");
            }
        }
        public ItemVariation GetItemVariation(long itemVariationId) {
            try {
                return _database.ItemVariations.Where(v => v.Id == itemVariationId).FirstOrDefault();
            }
            catch (Exception ex) {
                _logger.LogError(ex, $"An error occurred while retrieving item variation with ID {itemVariationId}.");
                throw new ApplicationException("Error retrieving item variation.");
            }
        }
        public ItemVariation CreateItemVariation(ItemVariation itemVariation) {
            try {
                _database.ItemVariations.Add(itemVariation);
                _database.SaveChanges();
                return itemVariation;
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred while creating item variation.");
                throw new ApplicationException("An unexpected error occurred while creating item variation.");
            }
        }
        public ItemVariation UpdateItemVariation(ItemVariation itemVariation) {
            try {
                var newItemVariation = (ItemVariation)itemVariation.Clone();
                _database.Add(newItemVariation);
                _database.Entry(itemVariation).CurrentValues.SetValues(_database.Entry(itemVariation).OriginalValues);
                itemVariation.IsArchived = true;
                _database.ItemVariations.Update(itemVariation);
                _database.SaveChanges();
                return newItemVariation;
            }
            catch (Exception ex) {
                _logger.LogError(ex, $"An error occurred while updating item variation with ID {itemVariation.Id}.");
                throw new ApplicationException("Error updating item variation.");
            }
        }
        public void DeleteItemVariation(long itemVariationId) {
            try {
                var itemVariation = _database.ItemVariations.Find(itemVariationId);
                if (itemVariation != null) {
                    itemVariation.IsArchived = true;
                    _database.ItemVariations.Update(itemVariation);
                    _database.SaveChanges();
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, $"An error occurred while deleting an item variation with ID {itemVariationId}.");
                throw new ApplicationException("Error deleting item variation.");
            }
        }
        public void DuplicateItemVariations(IEnumerable<ItemVariation> itemVariations, long itemId) {
            try {
                List<ItemVariation> copyList = [.. _database.ItemVariations
                    .Where(v => itemVariations.Select(v => v.Id).Contains(v.Id))
                    .Select(v => (ItemVariation)v.Clone())];
                foreach (ItemVariation copyItem in copyList)
                {
                    copyItem.ItemId = itemId;
                }
                _database.ItemVariations.AddRange(copyList);
                _database.ItemVariations
                    .Where(v => itemVariations.Select(v => v.Id).Contains(v.Id))
                    .ExecuteUpdate(v => v.SetProperty(x => x.IsArchived, x => true)); ;
                _database.SaveChanges();
            }
            catch (Exception ex) {
                _logger.LogError(ex, $"An error occurred while deleting item variations.");
                throw new ApplicationException("Error deleting item variations.");
            }
        }
        public IQueryable<ItemVariation> GetItemVariationQueriable() {
            return _database.ItemVariations;
        }

        public void DeleteItemVariations(IEnumerable<long> ids) {
            try {
                _database.ItemVariations
                    .Where(v => ids.Contains(v.Id))
                    .ExecuteUpdate(v => v.SetProperty(x => x.IsArchived, x => true)); ;
                _database.SaveChanges();
            }
            catch (Exception ex) {
                _logger.LogError(ex, $"An error occurred while deleting item variations.");
                throw new ApplicationException("Error deleting item variations.");
            }
        }
    }
}

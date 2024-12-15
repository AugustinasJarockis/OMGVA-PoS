using Microsoft.EntityFrameworkCore;
using OmgvaPOS.Database.Context;
using OmgvaPOS.Exceptions;
using OmgvaPOS.ItemVariationManagement.Models;

namespace OmgvaPOS.ItemVariationManagement.Repositories
{
    public class ItemVariationRepository(OmgvaDbContext database, ILogger<ItemVariationRepository> logger): IItemVariationRepository
    {
        private readonly OmgvaDbContext _database = database;
        private readonly ILogger<ItemVariationRepository> _logger = logger;

        public List<ItemVariation> GetItemVariations(long itemId) {
            return _database.ItemVariations
                .Where(v => v.IsArchived == false && v.ItemId == itemId)
                .ToList();
        }
        public ItemVariation? GetItemVariation(long itemVariationId) {
            return _database.ItemVariations
                .FirstOrDefault(v => v.Id == itemVariationId);
        }
        public ItemVariation CreateItemVariation(ItemVariation itemVariation) {
            _database.ItemVariations.Add(itemVariation);
            _database.SaveChanges();
            return itemVariation;
        }
        public ItemVariation UpdateItemVariation(ItemVariation itemVariation) {
            var newItemVariation = (ItemVariation)itemVariation.Clone();
            _database.Add(newItemVariation);
            _database.Entry(itemVariation).CurrentValues.SetValues(_database.Entry(itemVariation).OriginalValues);
            itemVariation.IsArchived = true;
            _database.ItemVariations.Update(itemVariation);
            _database.SaveChanges();
            return newItemVariation;
        }
        public void DeleteItemVariation(long itemVariationId) {
            var itemVariation = _database.ItemVariations.Find(itemVariationId);
            
            if (itemVariation == null) 
                throw new NotFoundException();
            
            itemVariation.IsArchived = true;
            _database.ItemVariations.Update(itemVariation);
            _database.SaveChanges();
        }
        public void DuplicateItemVariations(IEnumerable<ItemVariation> itemVariations, long itemId) {
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
        public IQueryable<ItemVariation> GetItemVariationQueriable() {
            return _database.ItemVariations;
        }

        public void DeleteItemVariations(IEnumerable<long> ids) {
            _database.ItemVariations
                .Where(v => ids.Contains(v.Id))
                .ExecuteUpdate(v => v.SetProperty(x => x.IsArchived, x => true)); ;
            _database.SaveChanges();
        }
    }
}

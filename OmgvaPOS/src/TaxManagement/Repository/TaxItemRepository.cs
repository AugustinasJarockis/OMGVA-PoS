using Microsoft.EntityFrameworkCore;
using OmgvaPOS.Database.Context;
using OmgvaPOS.TaxManagement.Models;
using OmgvaPOS.TaxManagement.Repository;

namespace OmgvaPOS.TaxManagement.Repository
{
    public class TaxItemRepository: ITaxItemRepository
    {
        private readonly OmgvaDbContext _context;
        private readonly ILogger<TaxRepository> _logger;

        public TaxItemRepository(OmgvaDbContext context, ILogger<TaxRepository> logger) {
            _context = context;
            _logger = logger;
        }

        public IQueryable<TaxItem> GetAllTaxItemQueriable() {
            return _context.TaxItems;
        }

        public void CreateConnectionsForNewItem(IEnumerable<TaxItem> taxItems, long itemId) {
            try {
                var copyTaxItems = taxItems.Select(t => new TaxItem() { ItemId = itemId, TaxId = t.Id });
                _context.TaxItems.AddRange(copyTaxItems);
                _context.SaveChanges();
            }
            catch (Exception ex) {
                _logger.LogError(ex, $"An error occurred while creating connections between taxes and new item.");
                throw new ApplicationException("Error creating connections between taxes and new item.");
            }
        }

        public void UpdateTaxItemTaxIds(IEnumerable<long> taxItemIds, long valueToSet) {
            try {
                _context.TaxItems
                    .Where(t => taxItemIds.Contains(t.Id))
                    .ExecuteUpdate(t => t.SetProperty(x => x.TaxId, x => valueToSet)); ;
                _context.SaveChanges();
            }
            catch (Exception ex) {
                _logger.LogError(ex, $"An error occurred while updating connections between taxes and items.");
                throw new ApplicationException("Error updating connections between taxes and items.");
            }
        }

        public void DeleteTaxItems(IEnumerable<long> ids) {
            try {
                _context.TaxItems
                    .Where(t => ids.Contains(t.Id))
                    .ExecuteDelete(); ;
                _context.SaveChanges();
            }
            catch (Exception ex) {
                _logger.LogError(ex, $"An error occurred while deleting connections between taxes and items.");
                throw new ApplicationException("Error deleting connections between taxes and items.");
            }
        }
    }
}

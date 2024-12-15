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

        // creating connections between taxes and an item.
        public void AddConnectionsBetweenItemAndTaxes(IEnumerable<long> taxIds, long itemId) {
            var newTaxItems = taxIds.Select(taxId => new TaxItem() { ItemId = itemId, TaxId = taxId });
            _context.TaxItems.AddRange(newTaxItems);
            _context.SaveChanges();
        }
        
        // creating connections between taxes and new item.
        public void CreateConnectionsForNewItem(IEnumerable<TaxItem> taxItems, long itemId) {
            var copyTaxItems = taxItems.Select(t => new TaxItem() { ItemId = itemId, TaxId = t.TaxId });
            _context.TaxItems.AddRange(copyTaxItems);
            _context.SaveChanges();
        }

        // updating connections between taxes and items.
        public void UpdateTaxItemTaxIds(IEnumerable<long> taxItemIds, long valueToSet) {
            _context.TaxItems
                .Where(t => taxItemIds.Contains(t.Id))
                .ExecuteUpdate(t => t.SetProperty(x => x.TaxId, x => valueToSet)); ;
            _context.SaveChanges();
        }

        // deleting connections between taxes and items.
        public void DeleteTaxItems(IEnumerable<long> ids) {
            _context.TaxItems
                .Where(t => ids.Contains(t.Id))
                .ExecuteDelete();
            _context.SaveChanges();
        }
    }
}

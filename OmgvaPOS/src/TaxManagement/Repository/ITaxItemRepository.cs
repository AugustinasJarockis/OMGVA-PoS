using OmgvaPOS.ItemManagement.Models;
using OmgvaPOS.TaxManagement.Models;

namespace OmgvaPOS.TaxManagement.Repository
{
    public interface ITaxItemRepository
    {
        public IQueryable<TaxItem> GetAllTaxItemQueriable();
        public void AddConnectionsBetweenItemAndTaxes(IEnumerable<long> taxIds, long itemId);
        public void CreateConnectionsForNewItem(IEnumerable<TaxItem> taxItems, long itemId);
        public void UpdateTaxItemTaxIds(IEnumerable<long> taxItemIds, long valueToSet);
        public void DeleteTaxItems(IEnumerable<long> ids);
    }
}

using OmgvaPOS.ItemVariationManagement.Models;

namespace OmgvaPOS.ItemVariationManagement.Repositories
{
    public interface IItemVariationRepository
    {
        public List<ItemVariation> GetItemVariations(long itemId);
        public ItemVariation GetItemVariation(long itemVariationId);
        public ItemVariation CreateItemVariation(ItemVariation itemVariation);
        public ItemVariation UpdateItemVariation(ItemVariation itemVariation);
        public void UpdateItemVariationInventoryQuantity(ItemVariation itemVariation);
        public void DeleteItemVariation(long itemVariationId);
        public void DuplicateItemVariations(IEnumerable<ItemVariation> itemVariations, long itemId);
        public IQueryable<ItemVariation> GetItemVariationQueriable();
        public void DeleteItemVariations(IEnumerable<long> ids);
    }
}

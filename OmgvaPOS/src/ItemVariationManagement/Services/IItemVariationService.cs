using OmgvaPOS.ItemVariationManagement.DTOs;

namespace OmgvaPOS.ItemVariationManagement.Services
{
    public interface IItemVariationService
    {
        public List<ItemVariationDTO> GetItemVariations(long itemId);
        public ItemVariationDTO GetItemVariation(long id);
        public long GetItemVariationBusinessNoException(long id);
        public ItemVariationDTO CreateItemVariation(ItemVariationCreationRequest itemVariationCreationRequest, long itemId);
        public ItemVariationDTO UpdateItemVariation(ItemVariationDTO itemVariation);
        public void DeleteItemVariation(long id);
    }
}

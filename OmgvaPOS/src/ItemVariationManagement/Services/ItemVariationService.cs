using OmgvaPOS.ItemManagement.Repositories;
using OmgvaPOS.ItemVariationManagement.DTOs;
using OmgvaPOS.ItemVariationManagement.Mappers;
using OmgvaPOS.ItemVariationManagement.Models;
using OmgvaPOS.ItemVariationManagement.Repositories;

namespace OmgvaPOS.ItemVariationManagement.Services
{
    public class ItemVariationService(
        IItemVariationRepository itemVariationRepository,
        IItemRepository itemRepository,
        ILogger<ItemVariationService> logger
        ) : IItemVariationService
    {
        private readonly IItemVariationRepository _itemVariationRepository = itemVariationRepository;
        private readonly IItemRepository _itemRepository = itemRepository;
        private readonly ILogger<ItemVariationService> _logger = logger;

        public List<ItemVariationDTO> GetItemVariations(long itemId) {
            var itemVariations = _itemVariationRepository.GetItemVariations(itemId);
            var itemVariationDTOs = itemVariations.Select(i => i.ToItemVariationDTO()).ToList();
            return itemVariationDTOs;
        }
        public ItemVariationDTO? GetItemVariation(long id) {
            var itemVariation = _itemVariationRepository.GetItemVariation(id);
            return itemVariation.ToItemVariationDTO();
        }

        public long GetItemVariationBusinessNoException(long id) {
            // TODO: item variantion may be null. 
            return _itemRepository.GetItem(_itemVariationRepository.GetItemVariation(id).ItemId).BusinessId;
        }
        public ItemVariationDTO CreateItemVariation(ItemVariationCreationRequest itemVariationCreationRequest, long itemId) {
            ItemVariation itemVariation = itemVariationCreationRequest.ToItemVariation();
            itemVariation.ItemId = itemId;
            var newItemVariation = _itemVariationRepository.CreateItemVariation(itemVariation);
            return newItemVariation.ToItemVariationDTO();
        }
        public ItemVariationDTO UpdateItemVariation(ItemVariationUpdateRequest itemVariationUpdateRequest, long id) {
            var itemVariation = _itemVariationRepository.GetItemVariation(id);
            itemVariation = itemVariationUpdateRequest.ToItemVariation(itemVariation);
            return _itemVariationRepository.UpdateItemVariation(itemVariation).ToItemVariationDTO();
        }
        public void DeleteItemVariation(long id) {
            _itemVariationRepository.DeleteItemVariation(id);
        }
    }
}

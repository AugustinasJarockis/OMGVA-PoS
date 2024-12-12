using OmgvaPOS.BusinessManagement.Models;
using OmgvaPOS.ItemManagement.DTOs;
using OmgvaPOS.ItemManagement.Models;
using OmgvaPOS.ItemManagement.Repositories;
using OmgvaPOS.ItemManagement.Services;
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
        public ItemVariationDTO GetItemVariation(long id) {
            var itemVariation = _itemVariationRepository.GetItemVariation(id);
            return itemVariation.ToItemVariationDTO();
        }

        public long GetItemVariationBusinessNoException(long id) {
            try {
                return _itemRepository.GetItem(_itemVariationRepository.GetItemVariation(id).ItemId).BusinessId;
            }
            catch (Exception ex) {
                _logger.LogError("Unexpected error occurred when trying to get item variation business id: " + ex);
                return -1;
            }
        }
        public ItemVariationDTO CreateItemVariation(ItemVariationCreationRequest itemVariationCreationRequest, long itemId) {
            ItemVariation itemVariation = itemVariationCreationRequest.ToItemVariation();
            var newItemVariation = _itemVariationRepository.CreateItemVariation(itemVariation);
            return newItemVariation.ToItemVariationDTO();
        }
        public ItemVariationDTO UpdateItemVariation(ItemVariationDTO itemVariationDTO) {
            var itemVariation = itemVariationDTO.ToItemVariation();
            return _itemVariationRepository.UpdateItemVariation(itemVariation).ToItemVariationDTO();
        }
        public void DeleteItemVariation(long id) {
            _itemVariationRepository.DeleteItemVariation(id);
        }
    }
}

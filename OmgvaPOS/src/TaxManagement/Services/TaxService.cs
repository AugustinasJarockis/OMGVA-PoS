using OmgvaPOS.ItemManagement.Repositories;
using OmgvaPOS.ItemManagement.Services;
using OmgvaPOS.TaxManagement.Mappers;
using OmgvaPOS.TaxManagement.Models;
using OmgvaPOS.TaxManagement.Repository;
using System.Collections;

namespace OmgvaPOS.TaxManagement.Services
{
    public class TaxService(
        ITaxItemRepository taxItemRepository,
        ITaxRepository taxRepository,
        IItemRepository itemRepository,
        IItemService itemService
        ): ITaxService
    {
        private readonly ITaxItemRepository _taxItemRepository = taxItemRepository;
        private readonly ITaxRepository _taxRepository = taxRepository;
        private readonly IItemRepository _itemRepository = itemRepository;
        private readonly IItemService _itemService = itemService;

        public void CreateTax(Tax tax) {
            _taxRepository.SaveTax(tax);
        }

        public Tax? GetTaxById(long id) {
            return _taxRepository.GetTaxById(id);
        }

        public List<Tax> GetAllTaxes() {
           return _taxRepository.GetAllTaxes();
        }

        public TaxDto UpdateTax(Tax tax) { //TODO: return new ID and update items
            var taxItemConnections = _taxItemRepository.GetAllTaxItemQueriable().Where(c => c.TaxId == tax.Id);
            var items = _itemRepository.GetItemsQueriable().Where(i => i.IsArchived == false);
            var itemsRelatedToTax = items.Where(i => taxItemConnections.Select(c => c.ItemId).Contains(i.Id));
            long oldId = tax.Id;

            _itemService.DuplicateItems(itemsRelatedToTax);

            Tax newTax = _taxRepository.UpdateTax(tax);

            _taxItemRepository.UpdateTaxItemTaxIds(
                taxItemConnections.Where(c => c.TaxId == oldId && items.Select(i => i.Id).Contains(c.ItemId)).Select(c => c.Id),
                newTax.Id
                );

            return TaxMapper.ToDTO(newTax);
        }

        public void DeleteTax(long id) {
            var taxItemConnections = _taxItemRepository.GetAllTaxItemQueriable().Where(c => c.TaxId == id);
            var items = _itemRepository.GetItemsQueriable().Where(i => i.IsArchived == false);
            var itemsRelatedToTax = items.Where(i => taxItemConnections.Select(c => c.ItemId).Contains(i.Id));
            
            _itemService.DuplicateItems(itemsRelatedToTax);
            _taxItemRepository.DeleteTaxItems(
                taxItemConnections.Where(c => items.Select(i => i.Id).Contains(c.ItemId)).Select(c => c.Id)
                );

            _taxRepository.DeleteTax(id);
        }
    }
}

using OmgvaPOS.DiscountManagement.Enums;
using OmgvaPOS.DiscountManagement.Models;
using OmgvaPOS.DiscountManagement.DTOs;
using OmgvaPOS.DiscountManagement.Mappers;
using OmgvaPOS.DiscountManagement.Repository;
using OmgvaPOS.ItemManagement.Repositories;
using OmgvaPOS.ItemManagement.Mappers;
using OmgvaPOS.ItemManagement.Services;

namespace OmgvaPOS.DiscountManagement.Service;

public class DiscountService : IDiscountService
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IItemService _itemService;
    private readonly ILogger<DiscountService> _logger;
    public DiscountService(
        IDiscountRepository discountRepository, 
        IItemRepository itemRepository,
        IItemService itemService,
        ILogger<DiscountService> logger
        ) {
        _discountRepository = discountRepository;
        _itemRepository = itemRepository;
        _itemService = itemService;
        _logger = logger;
    }

    public DiscountDTO CreateDiscount(CreateDiscountRequest request) {
        try {
            DiscountValidator.ValidateDateCreate(request.TimeValidUntil);
            DiscountValidator.ValidateDiscountAmount(request.Amount);
            DiscountValidator.ValidateDiscountType(request);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error while validating create discount request.");
            throw;
        }

        var discount = DiscountMapper.FromCreateDiscountRequest(request);
        _discountRepository.AddDiscount(discount);
        
        //TODO:
        if (request.Type == DiscountType.Order) {
            throw new NotImplementedException("Order is not implemented yet");
            //var order = _orderService.GetOrderById(request.OrderId);
            //order.DiscountId = discount.Id;
            //_orderService.UpdateOrder(order);
        }

        return DiscountMapper.ToDTO(discount);
    }



    public List<DiscountDTO> GetGlobalDiscounts() {
        List<Discount> discounts = _discountRepository.GetGlobalDiscounts();
        return discounts.Select(DiscountMapper.ToDTO).ToList();
    }
    public List<DiscountDTO> GetBusinessDiscounts(long businessId) {
        List<Discount> discounts = _discountRepository.GetBusinessDiscounts(businessId);
        return discounts.Select(DiscountMapper.ToDTO).ToList();
    }

    public DiscountDTO GetDiscountById(long id) {
        Discount discount = _discountRepository.GetDiscount(id);
        if (discount == null) 
            throw new Exception("Discount not found.");
        return DiscountMapper.ToDTO(discount);
    }

    public Discount GetDiscountNoException(long id) {
        try {
            return _discountRepository.GetDiscount(id);
        }
        catch (Exception ex) {
            _logger.LogError("Unexpected error occurred when trying to get item: " + ex);
            return null;
        }
    }

    public void UpdateDiscountValidUntil(long id, DateTime newValidUntil) {
        DiscountValidator.ValidateDateUpdate(newValidUntil);

        Discount discount = _discountRepository.GetDiscount(id);
        if (discount == null)
            throw new Exception("Discount not found.");
        if (discount.IsArchived)
            throw new Exception("Cannot update Valid Until date for an archived discount");

        discount.TimeValidUntil = newValidUntil;
        _discountRepository.UpdateDiscount(discount);
    }

    public void ArchiveDiscount(long id) {
        Discount discount = _discountRepository.GetDiscount(id);
        if (discount == null)
            throw new Exception("Discount not found.");
        if (discount.IsArchived)
            throw new Exception("Already archived.");

        discount.IsArchived = true;
        _discountRepository.UpdateDiscount(discount);

        // remove that discount from all items
        var items = _itemRepository.GetItemsQueriable()
            .Where(i => i.DiscountId == discount.Id);
        foreach (var item in items) {
            item.DiscountId = null;
            var itemDTO = item.ToItemDTO();
            _itemService.UpdateItem(itemDTO);
        }
    }

    public void UpdateDiscountOfItem(long discountId, long itemId) {
        Discount discount = _discountRepository.GetDiscount(discountId);
        if (discount == null)
            throw new Exception("Discount not found.");
        if (discount.IsArchived)
            throw new Exception("Cannot assign archived discount to item.");
        
        var item = _itemRepository.GetItem(itemId);
        if (item == null)
            throw new Exception("Item not found.");
        if (item.IsArchived)
            throw new Exception("Cannot assign discount to an archived item.");

        if (item.DiscountId == discountId)
            item.DiscountId = null;
        else 
            item.DiscountId = discountId;

        var itemDTO = item.ToItemDTO();
        _itemService.UpdateItem(itemDTO);
    }

    public DiscountDTO GetItemDiscount(long discountId) {
        throw new NotImplementedException();
    }

    public DiscountDTO GetOrderDiscount(long orderId) {
        throw new NotImplementedException();
    }




}

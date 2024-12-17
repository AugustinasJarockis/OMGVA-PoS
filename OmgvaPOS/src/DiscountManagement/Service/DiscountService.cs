using OmgvaPOS.DiscountManagement.Enums;
using OmgvaPOS.DiscountManagement.Models;
using OmgvaPOS.DiscountManagement.DTOs;
using OmgvaPOS.DiscountManagement.Mappers;
using OmgvaPOS.DiscountManagement.Repository;
using OmgvaPOS.ItemManagement.Repositories;
using OmgvaPOS.ItemManagement.Mappers;
using OmgvaPOS.ItemManagement.Services;
using OmgvaPOS.OrderManagement.Repository;
using OmgvaPOS.OrderManagement.Validators;

namespace OmgvaPOS.DiscountManagement.Service;

public class DiscountService : IDiscountService
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IItemService _itemService;
    private readonly IOrderRepository _orderRepository;
    public DiscountService(
        IDiscountRepository discountRepository, 
        IItemRepository itemRepository,
        IItemService itemService,
        IOrderRepository orderRepository
        ) {
        _discountRepository = discountRepository;
        _itemRepository = itemRepository;
        _itemService = itemService;
        _orderRepository = orderRepository;
    }

    public DiscountDTO CreateDiscount(CreateDiscountRequest request) {
        DiscountValidator.ValidateDateCreate(request.TimeValidUntil);
        DiscountValidator.ValidateDiscountAmount(request.Amount);
        DiscountValidator.ValidateDiscountType(request);

        var discount = DiscountMapper.FromCreateDiscountRequest(request);
        _discountRepository.AddDiscount(discount);
        
        if (request.Type == DiscountType.Order) {
            var order = _orderRepository.GetOrderNoAppendages((long)request.OrderId);
            OrderValidator.Exists(order);
            OrderValidator.IsOpen(order);

            order.DiscountId = discount.Id;
            _orderRepository.UpdateOrder(order);
        }

        return discount.ToDTO();
    }


    public List<DiscountDTO> GetBusinessDiscounts(long businessId) {
        List<Discount> discounts = _discountRepository.GetBusinessDiscounts(businessId);
        DiscountValidator.Exist(discounts);
        return discounts.Select(DiscountMapper.ToDTO).ToList();
    }

    public DiscountDTO? GetDiscountById(long id) {
        var discount = _discountRepository.GetDiscount(id);
        DiscountValidator.Exists(discount);
        return discount.ToDTO();
    }

    public long GetDiscountBusinessId(long discountId) {
        var discount = _discountRepository.GetDiscount(discountId);
        DiscountValidator.Exists(discount);
        return discount.BusinessId;
    }
    
    public void UpdateDiscountValidUntil(long discountId, DateTime newValidUntil) {
        DiscountValidator.ValidateDateUpdate(newValidUntil);

        var discount = _discountRepository.GetDiscount(discountId);
        DiscountValidator.Exists(discount);
        DiscountValidator.IsNotArchived(discount);

        discount.TimeValidUntil = newValidUntil;
        _discountRepository.UpdateDiscount(discount);
    }

    public void ArchiveDiscount(long id) {
        var discount = _discountRepository.GetDiscount(id);
        DiscountValidator.Exists(discount);
        DiscountValidator.IsNotArchived(discount);
        DiscountValidator.IsItemDiscount(discount);

        discount.IsArchived = true;
        _discountRepository.UpdateDiscount(discount);

        // remove that discount from all items
        var items = _itemRepository.GetItemsQueriable()
            .Where(i => i.DiscountId == discount.Id);
        foreach (var item in items) {
            item.DiscountId = null;
            var itemDTO = item.ToItemDTO();
            _itemService.UpdateItem(itemDTO, item.Id);
        }
    }

    public void UpdateOrderDiscountAmount(long discountId, long orderId, short amount) {
        var discount = _discountRepository.GetDiscount(discountId);
        DiscountValidator.Exists(discount);
        DiscountValidator.IsNotArchived(discount);

        var order = _orderRepository.GetOrderNoAppendages(orderId);
        OrderValidator.Exists(order);
        OrderValidator.IsOpen(order);

        discount.Amount = amount;
        _discountRepository.UpdateDiscount(discount);
    }
}

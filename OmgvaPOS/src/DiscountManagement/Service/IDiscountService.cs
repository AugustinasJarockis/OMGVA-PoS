using OmgvaPOS.DiscountManagement.Models;
using OmgvaPOS.DiscountManagement.DTOs;

namespace OmgvaPOS.DiscountManagement.Service
{
    public interface IDiscountService
    {
        public DiscountDTO CreateDiscount(CreateDiscountRequest createDiscountRequest);
        public List<DiscountDTO> GetGlobalDiscounts();
        public List<DiscountDTO> GetBusinessDiscounts(long businessId);
        public DiscountDTO GetDiscountById(long id);
        public Discount GetDiscountNoException(long id);
        public void UpdateDiscountValidUntil(long id, DateTime newValidUntil);
        public void ArchiveDiscount(long id);
        public void UpdateDiscountOfItem(long discountId, long itemId);
        public DiscountDTO GetOrderDiscount(long orderId);
        public DiscountDTO GetItemDiscount(long discountId);
    }
}
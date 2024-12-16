using OmgvaPOS.DiscountManagement.Models;
using OmgvaPOS.DiscountManagement.DTOs;

namespace OmgvaPOS.DiscountManagement.Service
{
    public interface IDiscountService
    {
        public DiscountDTO CreateDiscount(CreateDiscountRequest createDiscountRequest);
        public List<DiscountDTO> GetBusinessDiscounts(long businessId);
        public DiscountDTO? GetDiscountById(long id);
        public void UpdateDiscountValidUntil(long id, DateTime newValidUntil);
        public void ArchiveDiscount(long id);
        public long GetDiscountBusinessId(long discountId);
        public void UpdateOrderDiscountAmount(long discountId, long orderId, short amount);
    }
}
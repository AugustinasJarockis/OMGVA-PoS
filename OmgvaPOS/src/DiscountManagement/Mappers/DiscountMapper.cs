using OmgvaPOS.DiscountManagement.Models;
using OmgvaPOS.src.DiscountManagement.DTOs;

namespace OmgvaPOS.src.DiscountManagement.Mappers
{
    public class DiscountMapper
    {
        public static Discount FromCreateDiscountRequest(CreateDiscountRequest createDiscountRequest) {
            return new Discount() {
                Amount = createDiscountRequest.Amount,
                TimeValidUntil = createDiscountRequest.TimeValidUntil,
                Type = createDiscountRequest.Type,
                IsArchived = false,
                BusinessId = (long)createDiscountRequest.BusinessId
            };
        }

        public static DiscountDTO ToDTO(Discount discount) {
            return new DiscountDTO() {
                Id = discount.Id,
                Amount = discount.Amount,
                TimeValidUntil = discount.TimeValidUntil,
                Type = discount.Type,
                IsArchived = discount.IsArchived
            };
        }
    }
}

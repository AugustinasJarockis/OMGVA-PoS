﻿using OmgvaPOS.DiscountManagement.Models;
using OmgvaPOS.DiscountManagement.DTOs;

namespace OmgvaPOS.DiscountManagement.Mappers
{
    public static class DiscountMapper
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

        public static DiscountDTO ToDTO(this Discount discount)
        {
            return new DiscountDTO() {
                Id = discount.Id,
                Amount = discount.Amount,
                TimeValidUntil = discount.TimeValidUntil,
                Type = discount.Type
            };
        }
    }
}

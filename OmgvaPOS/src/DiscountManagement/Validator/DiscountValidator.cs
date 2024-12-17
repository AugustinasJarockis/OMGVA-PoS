using OmgvaPOS.DiscountManagement.Enums;
using OmgvaPOS.DiscountManagement.DTOs;
using OmgvaPOS.Exceptions;
using OmgvaPOS.DiscountManagement.Models;

namespace OmgvaPOS.DiscountManagement.Service;

public static class DiscountValidator
{
    public static void ValidateDateCreate(DateTime dateTime) {
        if (dateTime.Date < DateTime.Today) {
            throw new ValidationException("A new discount must be valid at today's date.");
        }
    }

    public static void ValidateDateUpdate(DateTime dateTime) {
        if (dateTime.Date < DateTime.Today) {
            throw new ValidationException("Cannot set \"Valid until:\" to a date in the past, to deactivate discount, archive it.");
        }
    }

    public static void ValidateDiscountAmount(short amount) {
        if (amount <= 0 || amount > 100) {
            throw new ValidationException("Discount amount must be between 0 and 100.");
        }
    }

    public static void ValidateDiscountType(CreateDiscountRequest request) {
        if (request.Type == DiscountType.Item && request.OrderId != null) {
            throw new ValidationException("Some type of Id was sent while creating a discount for an item.");
        }
        else if (request.Type == DiscountType.Order && request.OrderId == null) {
            throw new ValidationException("Cannot create a discount for an order without OrderId.");
        }
    }

    public static void Exists(object discount) {
        if (discount == null) {
            throw new NotFoundException("Discount not found");
        }
    }

    public static void IsNotArchived(Discount discount) {
        if (discount.IsArchived) {
            throw new ValidationException("Discount is archived");
        }
    }

    public static void IsItemDiscount(Discount discount) {
        if (discount.Type == DiscountType.Order) {
            throw new ValidationException("Cannot archive an order discount");
        }
    }

    internal static void Exist(List<Discount> discounts) {
        if (discounts == null || !discounts.Any())
            throw new NotFoundException("No discounts found");
    }
}

using OmgvaPOS.DiscountManagement.Enums;
using OmgvaPOS.DiscountManagement.DTOs;
using System.ComponentModel.DataAnnotations;

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
}

using OmgvaPOS.PaymentManagement.DTOs;
using OmgvaPOS.PaymentManagement.Enums;
using OmgvaPOS.PaymentManagement.Models;

namespace OmgvaPOS.PaymentManagement.Mappers;

public static class PaymentMapper
{
    public static Payment ToPayment(this PaymentDTO request)
    {
        if (request.Id != null)
        {
            return new Payment
            {
                Id = request.Id,
                Method = Enum.Parse<PaymentMethod>(request.Method, true),
                CustomerId = request.CustomerId,
                OrderId = request.OrderId,
                Amount = request.Amount,
                GiftCardId = request.GiftCardId
            };
        }
        return new Payment
        {
            Id = Guid.NewGuid().ToString(),
            Method = Enum.Parse<PaymentMethod>(request.Method, true),
            CustomerId = request.CustomerId,
            OrderId = request.OrderId,
            Amount = request.Amount,
            GiftCardId = request.GiftCardId
        };
    }

    public static PaymentDTO ToPaymentDTO(this Payment payment)
    {
        return new PaymentDTO
        {
            Id = payment.Id,
            Method = payment.Method.ToString(),
            CustomerId = payment.CustomerId,
            OrderId = payment.OrderId,
            Amount = payment.Amount,
            GiftCardId = payment.GiftCardId
        };
    }
}
using OmgvaPOS.PaymentManagement.DTOs;
using OmgvaPOS.PaymentManagement.Enums;
using OmgvaPOS.PaymentManagement.Models;

namespace OmgvaPOS.PaymentManagement.Mappers;

public static class PaymentMapper
{
    public static Payment ToPayment(this PaymentRequest request)
    {
        return new Payment
        {
            Id = Guid.NewGuid().ToString(),
            Method = Enum.Parse<PaymentMethod>(request.PaymentMethodId, true),
            CustomerId = request.CustomerId,
            OrderId = request.OrderId,
            Amount = request.Amount,
            GiftCardPaymentId = request.GiftCardPaymentId
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
            GiftCardPaymentId = payment.GiftCardPaymentId
        };
    }
}
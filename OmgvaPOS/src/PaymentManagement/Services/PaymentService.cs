using Microsoft.AspNetCore.Http.HttpResults;
using OmgvaPOS.BusinessManagement.DTOs;
using OmgvaPOS.BusinessManagement.Models;
using OmgvaPOS.BusinessManagement.Services;
using OmgvaPOS.Exceptions;
using OmgvaPOS.GiftcardManagement.DTOs;
using OmgvaPOS.GiftcardManagement.Service;
using OmgvaPOS.PaymentManagement.DTOs;
using OmgvaPOS.PaymentManagement.Enums;
using OmgvaPOS.PaymentManagement.Mappers;
using OmgvaPOS.PaymentManagement.Models;
using OmgvaPOS.PaymentManagement.Repository;

namespace OmgvaPOS.PaymentManagement.Services;

public class PaymentService(
    IPaymentRepository paymentRepository,
    IBusinessService businessService,
    IGiftcardService giftcardService,
    ILogger<PaymentService> logger) : IPaymentService
{
    public List<PaymentDTO> GetPayments()
    {
        return paymentRepository.GetPayments()
            .Select(p => p.ToPaymentDTO())
            .ToList();
    }

    public List<PaymentDTO> GetPayment(long orderId)
    {
        return paymentRepository.GetPayment(orderId)
            .Select(p => p.ToPaymentDTO())
            .ToList();
    }

    public PaymentDTO CreatePayment(PaymentDTO request)
    {
        var payment = request.ToPayment();
        return paymentRepository.CreatePayment(payment).ToPaymentDTO();
    }
    
    public PaymentDTO ProcessGiftcardPayment(PaymentRequest request)
    {
        if (giftcardService.GetGiftcard(code: request.GiftCardCode) == null)
            throw new BadRequestException("Incorrect giftcard code");
        
        var updateRequest = new GiftcardUpdateRequest
        {
            Code = request.GiftCardCode,
            Amount = request.Amount
        };
        giftcardService.UpdateGiftcard(updateRequest);
        
        var payment = new Payment
        {
            Id = Guid.NewGuid().ToString(),
            Amount = request.Amount,
            OrderId = request.OrderId,
            Method = PaymentMethod.Giftcard,
            CustomerId = request.CustomerId
        };
        return paymentRepository.CreatePayment(payment).ToPaymentDTO();
    }
}
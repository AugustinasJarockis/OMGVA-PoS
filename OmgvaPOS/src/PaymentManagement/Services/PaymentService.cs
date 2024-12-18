using OmgvaPOS.BusinessManagement.DTOs;
using OmgvaPOS.BusinessManagement.Models;
using OmgvaPOS.BusinessManagement.Services;
using OmgvaPOS.PaymentManagement.DTOs;
using OmgvaPOS.PaymentManagement.Mappers;
using OmgvaPOS.PaymentManagement.Models;
using OmgvaPOS.PaymentManagement.Repository;

namespace OmgvaPOS.PaymentManagement.Services;

public class PaymentService(IPaymentRepository paymentRepository, IBusinessService businessService, ILogger<PaymentService> logger) : IPaymentService
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
    public BusinessDTO GetBusinessById(long businessId)
    {
        return businessService.GetBusiness(businessId);
    }
}
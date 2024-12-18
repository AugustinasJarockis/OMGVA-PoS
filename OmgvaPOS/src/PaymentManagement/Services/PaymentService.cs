using OmgvaPOS.BusinessManagement.DTOs;
using OmgvaPOS.BusinessManagement.Models;
using OmgvaPOS.BusinessManagement.Services;
using OmgvaPOS.PaymentManagement.Models;
using OmgvaPOS.PaymentManagement.Repository;

namespace OmgvaPOS.PaymentManagement.Services;

public class PaymentService(IPaymentRepository paymentRepository, IBusinessService businessService, ILogger<PaymentService> logger) : IPaymentService
{
    public List<Payment> GetPayments()
    {
        return paymentRepository.GetPayments();
    }
    public List<Payment> GetPayment(long orderId)
    {
        return paymentRepository.GetPayment(orderId);
    }
    public Payment CreatePayment(Payment payment)
    {
        return paymentRepository.CreatePayment(payment);
    }
    public BusinessDTO GetBusinessById(long businessId)
    {
        return businessService.GetBusiness(businessId);
    }
}
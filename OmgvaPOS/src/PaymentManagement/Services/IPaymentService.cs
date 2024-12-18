using OmgvaPOS.BusinessManagement.DTOs;
using OmgvaPOS.PaymentManagement.Models;

namespace OmgvaPOS.PaymentManagement.Services;

public interface IPaymentService
{
    public List<Payment> GetPayments();
    public List<Payment> GetPayment(long orderId);
    public Payment CreatePayment(Payment payment);
    public BusinessDTO GetBusinessById(long businessId);
}
using OmgvaPOS.BusinessManagement.DTOs;
using OmgvaPOS.PaymentManagement.DTOs;
using OmgvaPOS.PaymentManagement.Models;

namespace OmgvaPOS.PaymentManagement.Services;

public interface IPaymentService
{
    public List<PaymentDTO> GetPayments();
    public List<PaymentDTO> GetPayment(long orderId);
    public PaymentDTO CreatePayment(PaymentDTO request);
}
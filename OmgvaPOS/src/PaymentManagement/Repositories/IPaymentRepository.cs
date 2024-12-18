using OmgvaPOS.PaymentManagement.Models;

namespace OmgvaPOS.PaymentManagement.Repository;

public interface IPaymentRepository
{
    public List<Payment> GetPayments();
    public List<Payment> GetPayment(long orderId);
    public Payment CreatePayment(Payment payment);
}
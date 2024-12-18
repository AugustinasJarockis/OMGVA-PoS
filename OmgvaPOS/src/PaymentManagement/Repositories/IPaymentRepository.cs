using OmgvaPOS.PaymentManagement.Models;

namespace OmgvaPOS.PaymentManagement.Repository;

public interface IPaymentRepository
{
    public Payment CreatePayment(Payment payment);
}
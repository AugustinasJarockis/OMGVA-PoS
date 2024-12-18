using OmgvaPOS.Database.Context;
using OmgvaPOS.PaymentManagement.Models;

namespace OmgvaPOS.PaymentManagement.Repository;

public class PaymentRepository(OmgvaDbContext context, ILogger<PaymentRepository> logger) : IPaymentRepository
{
    private readonly ILogger<PaymentRepository> _logger = logger;

    public Payment CreatePayment(Payment payment)
    {
        context.Payments.Add(payment);
        context.SaveChanges();
        return payment;
    }
}
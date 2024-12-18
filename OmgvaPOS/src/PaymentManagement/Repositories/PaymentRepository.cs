using OmgvaPOS.Database.Context;
using OmgvaPOS.PaymentManagement.Models;

namespace OmgvaPOS.PaymentManagement.Repository;

public class PaymentRepository(OmgvaDbContext context, ILogger<PaymentRepository> logger) : IPaymentRepository
{
    private readonly ILogger<PaymentRepository> _logger = logger;

    public List<Payment> GetPayments()
    {
        return [.. context.Payments];
    }

    public List<Payment> GetPayment(long orderId)
    {
        return context.Payments
            .Where(p => p.OrderId == orderId)
            .ToList();
    }
    public Payment CreatePayment(Payment payment)
    {
        context.Payments.Add(payment);
        context.SaveChanges();
        return payment;
    }
}
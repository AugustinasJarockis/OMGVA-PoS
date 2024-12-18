namespace OmgvaPOS.PaymentManagement.DTOs;

public class PaymentDTO
{
    public string Id { get; set; }
    public string Method { get; set; }
    public long CustomerId { get; set; }
    public long OrderId { get; set; }
    public long Amount { get; set; }
    public long? GiftCardId { get; set; }
}
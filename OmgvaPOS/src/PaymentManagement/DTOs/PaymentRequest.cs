using System.ComponentModel.DataAnnotations;

namespace OmgvaPOS.PaymentManagement.DTOs;

public class PaymentRequest
{
    [Required]
    public string PaymentMethodId { get; set; }
    
    [Required]
    public long CustomerId { get; set; }
    
    public long? GiftCardPaymentId { get; set; }
    public string? GiftCardCode { get; set; }
    
    [Required]
    public long OrderId { get; set; }
    
    [Required]
    [Range(1, long.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public long Amount { get; set; }

}
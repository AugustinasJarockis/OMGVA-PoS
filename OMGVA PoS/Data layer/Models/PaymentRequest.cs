namespace OMGVA_PoS.Data_layer.Models;

public class PaymentRequest 
{
    public string PaymentMethodId { get; set; }
    public long Amount { get; set; }
}
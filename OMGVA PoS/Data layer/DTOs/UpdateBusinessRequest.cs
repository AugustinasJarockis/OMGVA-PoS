namespace OMGVA_PoS.Data_layer.DTOs
{
    public class UpdateBusinessRequest
    {
        public string? StripeAccId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}

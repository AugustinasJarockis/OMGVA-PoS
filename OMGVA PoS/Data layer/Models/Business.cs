namespace OMGVA_PoS.Data_layer.Models
{
    public class Business
    {
        public long Id { get; set; }
        public string StripeAccId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}

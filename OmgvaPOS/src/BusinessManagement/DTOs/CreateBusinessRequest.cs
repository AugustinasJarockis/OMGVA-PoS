namespace OmgvaPOS.BusinessManagement.DTOs
{
    public class CreateBusinessRequest
    {
        public string Name { get; set; }
        public string StripeSecretKey { get; set; }
        public string StripePublishKey { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}

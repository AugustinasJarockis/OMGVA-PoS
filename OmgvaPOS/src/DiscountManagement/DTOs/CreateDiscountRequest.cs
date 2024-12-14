using OmgvaPOS.DiscountManagement.Enums;
namespace OmgvaPOS.DiscountManagement.DTOs
{
    public class CreateDiscountRequest
    {
        public short Amount { get; set; }
        public DateTime TimeValidUntil { get; set; } 
        public DiscountType Type { get; set; }
        public long? OrderId { get; set; }
        public long? BusinessId { get; set; }
    }
}

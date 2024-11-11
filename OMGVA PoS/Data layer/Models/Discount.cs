using OMGVA_PoS.Data_layer.Enums;

namespace OMGVA_PoS.Data_layer.Models
{
    public class Discount
    {
        public long Id { get; set; }
        public short Amount { get; set; }
        public DateTime TimeValidUntil { get; set; }
        public DiscountType Type { get; set; }
        public bool IsArchived { get; set; }
    }
}

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

        // navigational properties
        public ICollection<Order> Orders { get; set; } // Discount can be applied to Orders
        public ICollection<Item> Items { get; set; } // Discount can be applied to Items
        public ICollection<OrderItem> OrderItems {  get; set; } // by proxy to items that are ordered
    }
}

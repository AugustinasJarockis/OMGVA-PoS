using OmgvaPOS.DiscountManagement.Enums;
using OmgvaPOS.ItemManagement.Models;
using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderManagement.Models;

namespace OmgvaPOS.DiscountManagement.Models
{
    public class Discount
    {
        public long Id { get; set; }
        public short Amount { get; set; }
        public DateTime TimeValidUntil { get; set; }
        public DiscountType Type { get; set; }
        public bool IsArchived { get; set; }

        // navigational properties
        public ICollection<OrderManagement.Models.Order> Orders { get; set; } // Discount can be applied to Orders
        public ICollection<Item> Items { get; set; } // Discount can be applied to Items
        public ICollection<OrderItem> OrderItems {  get; set; } // by proxy to items that are ordered
    }
}

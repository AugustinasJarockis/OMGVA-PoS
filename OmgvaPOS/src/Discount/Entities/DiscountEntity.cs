using OmgvaPOS.Discount.Enums;
using OmgvaPOS.Item.Entities;
using OmgvaPOS.Order.Entities;
using OmgvaPOS.OrderItem.Entities;

namespace OmgvaPOS.Discount.Entities
{
    public class DiscountEntity
    {
        public long Id { get; set; }
        public short Amount { get; set; }
        public DateTime TimeValidUntil { get; set; }
        public DiscountType Type { get; set; }
        public bool IsArchived { get; set; }

        // navigational properties
        public ICollection<OrderEntity> Orders { get; set; } // Discount can be applied to Orders
        public ICollection<ItemEntity> Items { get; set; } // Discount can be applied to Items
        public ICollection<OrderItemEntity> OrderItems {  get; set; } // by proxy to items that are ordered
    }
}

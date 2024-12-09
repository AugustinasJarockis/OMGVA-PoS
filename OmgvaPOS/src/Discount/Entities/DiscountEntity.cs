using OmgvaPOS.Discount.Enums;

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
        public ICollection<Order.Entities.OrderEntity> Orders { get; set; } // Discount can be applied to Orders
        public ICollection<Item.Entities.ItemEntity> Items { get; set; } // Discount can be applied to Items
        public ICollection<OrderItem.Entities.OrderItemEntity> OrderItems {  get; set; } // by proxy to items that are ordered
    }
}

using OmgvaPOS.Discount.Entities;
using OmgvaPOS.Order.Entities;
using OmgvaPOS.OrderItemVariation.Entities;

namespace OmgvaPOS.OrderItem.Entities
{
    public class OrderItemEntity
    {
        public long Id { get; set; }
        public short Quantity { get; set; }
        public long ItemId { get; set; }
        public long OrderId { get; set; }
        public long? DiscountId { get; set; }

        // navigational properties
        public ICollection<OrderItemVariationEntity> OrderItemVariations { get; set; } // OrderItem can have variations

        // for foreign keys
        public OrderEntity OrderEntity { get; set; }
        public DiscountEntity DiscountEntity { get; set; }
    }
}

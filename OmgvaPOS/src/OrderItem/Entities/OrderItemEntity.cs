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
        public ICollection<OrderItemVariation.Entities.OrderItemVariationEntity> OrderItemVariations { get; set; } // OrderItem can have variations

        // for foreign keys
        public Order.Entities.OrderEntity OrderEntity { get; set; }
        public Discount.Entities.DiscountEntity DiscountEntity { get; set; }
    }
}

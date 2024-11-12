namespace OMGVA_PoS.Data_layer.Models
{
    public class OrderItem
    {
        public long Id { get; set; }
        public short Quantity { get; set; }
        public long ItemId { get; set; }
        public long OrderId { get; set; }
        public long? DiscountId { get; set; }

        // navigational properties
        public ICollection<OrderItemVariation> OrderItemVariations { get; set; } // OrderItem can have variations

        // for foreign keys
        public Order Order { get; set; }
        public Discount Discount { get; set; }
    }
}

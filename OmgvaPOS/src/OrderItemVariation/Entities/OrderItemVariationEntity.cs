namespace OmgvaPOS.OrderItemVariation.Entities
{
    public class OrderItemVariationEntity
    {
        public long Id { get; set; }
        public long ItemVariationId { get; set; }
        public long OrderItemId { get; set; }

        // navigational properties
        // for foreign keys
        public ItemVariation.Entities.ItemVariationEntity ItemVariationEntity { get; set; }
        public OrderItem.Entities.OrderItemEntity OrderItemEntity { get; set; }
    }
}

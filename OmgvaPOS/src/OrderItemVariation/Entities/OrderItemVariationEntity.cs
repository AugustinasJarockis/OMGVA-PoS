using OmgvaPOS.ItemVariation.Entities;
using OmgvaPOS.OrderItem.Entities;

namespace OmgvaPOS.OrderItemVariation.Entities
{
    public class OrderItemVariationEntity
    {
        public long Id { get; set; }
        public long ItemVariationId { get; set; }
        public long OrderItemId { get; set; }

        // navigational properties
        // for foreign keys
        public ItemVariationEntity ItemVariationEntity { get; set; }
        public OrderItemEntity OrderItemEntity { get; set; }
    }
}

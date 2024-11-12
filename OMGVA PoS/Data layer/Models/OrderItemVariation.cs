namespace OMGVA_PoS.Data_layer.Models
{
    public class OrderItemVariation
    {
        public long Id { get; set; }
        public long ItemVariationId { get; set; }
        public long OrderItemId { get; set; }

        // navigational properties
        // for foreign keys
        public ItemVariation ItemVariation { get; set; }
        public OrderItem OrderItem { get; set; }
    }
}

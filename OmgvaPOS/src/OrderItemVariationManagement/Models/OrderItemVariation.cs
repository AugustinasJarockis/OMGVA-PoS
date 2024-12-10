using OmgvaPOS.ItemVariationManagement.Models;
using OmgvaPOS.OrderItemManagement.Models;

namespace OmgvaPOS.OrderItemVariationManagement.Models
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

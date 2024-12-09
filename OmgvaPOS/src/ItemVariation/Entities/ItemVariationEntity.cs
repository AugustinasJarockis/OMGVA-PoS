using OmgvaPOS.Item.Entities;
using OmgvaPOS.OrderItemVariation.Entities;

namespace OmgvaPOS.ItemVariation.Entities
{
    public class ItemVariationEntity
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public string Name { get; set; }
        public int InventoryQuantity { get; set; }
        public decimal PriceChange { get; set; }
        public string ItemVariationGroup { get; set; }
        public bool IsArchived { get; set; }

        // navigational properties
        public ICollection<OrderItemVariationEntity> OrderItemVariations { get; set; } // can be used in orders

        // for foreign keys
        public ItemEntity ItemEntity { get; set; } 
    }
}

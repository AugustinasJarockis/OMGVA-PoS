using OmgvaPOS.ItemManagement.Models;
using OmgvaPOS.OrderItemVariationManagement.Models;

namespace OmgvaPOS.ItemVariationManagement.Models
{
    public class ItemVariation: ICloneable
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public string Name { get; set; }
        public int InventoryQuantity { get; set; }
        public decimal PriceChange { get; set; }
        public string ItemVariationGroup { get; set; }
        public bool IsArchived { get; set; }

        // navigational properties
        public ICollection<OrderItemVariation> OrderItemVariations { get; set; } // can be used in orders

        // for foreign keys
        public Item Item { get; set; } 

        public object Clone() {
            return new ItemVariation {
                ItemId = this.ItemId,
                Name = this.Name,
                InventoryQuantity = this.InventoryQuantity,
                PriceChange = this.PriceChange,
                ItemVariationGroup = this.ItemVariationGroup,
                IsArchived = this.IsArchived
            };
        }
    }
}

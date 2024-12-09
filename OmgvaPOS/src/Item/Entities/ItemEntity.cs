using OmgvaPOS.BusinessManagement.Entities;
using OmgvaPOS.Discount.Entities;
using OmgvaPOS.ItemVariation.Entities;
using OmgvaPOS.TaxManagement.Entities;

namespace OmgvaPOS.Item.Entities
{
    public class ItemEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int InventoryQuantity { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public string ItemGroup { get; set; }
        public TimeSpan Duration { get; set; }
        public string ImgPath { get; set; }
        public bool IsArchived { get; set;}
        public long BusinessId { get; set; }
        public long? DiscountId { get; set; }

        // navigational properties
        public ICollection<TaxItemEntity> TaxItems { get; set; } // Item can have taxes
        public ICollection<ItemVariationEntity> ItemVariations { get; set; } // Item can have variations

        // for foreign keys
        public BusinessEntity BusinessEntity { get; set; } 
        public DiscountEntity DiscountEntity { get; set; }
    }
}

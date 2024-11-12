namespace OMGVA_PoS.Data_layer.Models
{
    public class Item
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
        public ICollection<TaxItem> TaxItems { get; set; } // Item can have taxes
        public ICollection<ItemVariation> ItemVariations { get; set; } // Item can have variations

        // for foreign keys
        public Business Business { get; set; } 
        public Discount Discount { get; set; }
    }
}

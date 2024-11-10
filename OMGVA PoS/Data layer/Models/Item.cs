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
    }
}

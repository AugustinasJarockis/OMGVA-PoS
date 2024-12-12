namespace OmgvaPOS.ItemManagement.DTOs
{
    public class CreateItemRequest
    {
        public string Name { get; set; }
        public int InventoryQuantity { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public string ItemGroup { get; set; }
        public TimeSpan? Duration { get; set; }
        public string ImgPath { get; set; }
        public long? DiscountId { get; set; }
        public long? UserId { get; set; }
    }
}

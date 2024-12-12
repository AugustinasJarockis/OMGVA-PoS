namespace OmgvaPOS.ItemVariationManagement.DTOs
{
    public class ItemVariationCreationRequest
    {
        public string Name { get; set; }
        public int InventoryQuantity { get; set; }
        public decimal PriceChange { get; set; }
        public string ItemVariationGroup { get; set; }
    }
}

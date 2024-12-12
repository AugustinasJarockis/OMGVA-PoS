namespace OmgvaPOS.ItemVariationManagement.DTOs
{
    public class ItemVariationDTO
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public string Name { get; set; }
        public int InventoryQuantity { get; set; }
        public decimal PriceChange { get; set; }
        public string ItemVariationGroup { get; set; } //TODO:  validate this group
    }
}

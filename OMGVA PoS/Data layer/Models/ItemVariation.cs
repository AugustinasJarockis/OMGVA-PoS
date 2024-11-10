namespace OMGVA_PoS.Data_layer.Models
{
    public class ItemVariation
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public string Name { get; set; }
        public int InventoryQuantity { get; set; }
        public decimal PriceChange { get; set; }
        public string ItemVariationGroup { get; set; }
        public bool IsArchived { get; set; }
    }
}

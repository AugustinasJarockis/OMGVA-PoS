namespace OmgvaPOS.TaxManagement.Entities
{
    public class TaxEntity
    {
        public long Id { get; set; }
        public string TaxType { get; set; }
        public short Percent { get; set; }
        public bool IsArchived { get; set; }

        // navigational properties
        public ICollection<TaxItemEntity> TaxItems { get; set; } // can have TaxItems (tax can be applied to some Items)
    }
}

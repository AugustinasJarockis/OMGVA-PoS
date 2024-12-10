namespace OmgvaPOS.TaxManagement.Models
{
    public class Tax
    {
        public long Id { get; set; }
        public string TaxType { get; set; }
        public short Percent { get; set; }
        public bool IsArchived { get; set; }

        // navigational properties
        public ICollection<TaxItem> TaxItems { get; set; } // can have TaxItems (tax can be applied to some Items)
    }
}

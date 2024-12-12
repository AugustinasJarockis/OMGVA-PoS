namespace OmgvaPOS.TaxManagement.Models
{
    public class Tax: ICloneable
    {
        public long Id { get; set; }
        public string TaxType { get; set; }
        public short Percent { get; set; }
        public bool IsArchived { get; set; }

        // navigational properties
        public ICollection<TaxItem> TaxItems { get; set; } // can have TaxItems (tax can be applied to some Items)

        public object Clone() {
            return new Tax() {
                TaxType = this.TaxType,
                Percent = this.Percent,
                IsArchived = this.IsArchived
            };
        }
    }
}

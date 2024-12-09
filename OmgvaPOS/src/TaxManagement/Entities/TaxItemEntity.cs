
namespace OmgvaPOS.TaxManagement.Entities
{
    public class TaxItemEntity
    {
        public long Id { get; set; }
        public long TaxId { get; set; }
        public long ItemId { get; set; }

        // navigational properties
        // for foreign keys
        public TaxEntity TaxEntity { get; set; }
        public Item.Entities.ItemEntity ItemEntity {  get; set; }
    }
}

namespace OmgvaPOS.ItemManagement.DTOs
{
    public class ChangeItemTaxesRequest
    {
        public List<long> TaxesToAddIds { get; set; } = [];
        public List<long> TaxesToRemoveIds { get; set; } = [];
    }
}

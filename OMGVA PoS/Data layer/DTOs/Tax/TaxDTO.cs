namespace OMGVA_PoS.Data_layer.DTOs;

public class TaxDTO
{
    public long Id { get; set; }
    public string TaxType { get; set; }
    public short Percent { get; set; }
    public bool IsArchived { get; set; }
}
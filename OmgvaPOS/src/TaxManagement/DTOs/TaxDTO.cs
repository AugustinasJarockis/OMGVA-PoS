namespace OmgvaPOS.TaxManagement.Models;

public class TaxDto
{
    public long Id { get; set; }
    public string TaxType { get; set; }
    public short Percent { get; set; }
}
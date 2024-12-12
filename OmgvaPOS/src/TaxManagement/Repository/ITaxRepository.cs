using OmgvaPOS.TaxManagement.Models;

namespace OmgvaPOS.TaxManagement.Repository;

public interface ITaxRepository
{
    public List<Tax> GetAllTaxes();
    public Tax GetTaxById(long id);
    public Tax SaveTax(Tax tax);
    public Tax UpdateTax(Tax tax);
    public void DeleteTax(long id);
}
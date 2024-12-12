using OmgvaPOS.TaxManagement.Models;

namespace OmgvaPOS.TaxManagement.Services
{
    public interface ITaxService
    {
        public void CreateTax(Tax tax);
        public Tax GetTaxById(long id);
        public List<Tax> GetAllTaxes();
        public TaxDto UpdateTax(Tax tax);
        public void DeleteTax(long id);
    }
}

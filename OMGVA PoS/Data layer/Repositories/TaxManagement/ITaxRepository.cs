using OMGVA_PoS.Data_layer.Models;

namespace OMGVA_PoS.Data_layer.Repositories.TaxManagement;

public interface ITaxRepository
{
    public Task<List<Tax>> GetAllTaxesAsync();
    public Task<Tax> GetTaxByIdAsync(long id);
    public Task<Tax> SaveTaxAsync(Tax tax);
    public Task UpdateTaxAsync(Tax tax);
    public Task DeleteTaxAsync(long id);
}
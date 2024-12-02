namespace OMGVA_PoS.Data_layer.Repositories.Tax;

public interface ITaxRepository
{
    public Task<List<Models.Tax>> GetAllTaxesAsync();
    public Task<Models.Tax> GetTaxByIdAsync(long id);
    public Task<Models.Tax> SaveTaxAsync(Models.Tax tax);
    public Task UpdateTaxAsync(Models.Tax tax);
    public Task DeleteTaxAsync(long id);
}
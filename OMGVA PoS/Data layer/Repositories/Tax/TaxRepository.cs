using Microsoft.EntityFrameworkCore;
using OMGVA_PoS.Data_layer.Context;

namespace OMGVA_PoS.Data_layer.Repositories.Tax;

public class TaxRepository(OMGVADbContext context) : ITaxRepository
{
    public async Task<List<Models.Tax>> GetAllTaxesAsync()
    {
        return await context.Taxes
            .ToListAsync();
    }

    public async Task<Models.Tax> GetTaxByIdAsync(long id)
    {
        return await context.Taxes.Where(tax => tax.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Models.Tax> SaveTaxAsync(Models.Tax tax)
    {
        context.Taxes.Add(tax);
        await context.SaveChangesAsync();
        return tax;
    }

    public async Task UpdateTaxAsync(Models.Tax tax)
    {
        context.Taxes.Update(tax);
        await context.SaveChangesAsync();
    }

    public async Task DeleteTaxAsync(long id)
    {
        var tax = await context.Taxes.SingleOrDefaultAsync(tax => tax.Id == id);
        if (tax != null)
        {
            context.Taxes.Remove(tax);
            await context.SaveChangesAsync();
        }
    }
    
}
using Microsoft.EntityFrameworkCore;
using OmgvaPOS.Database.Context;
using OmgvaPOS.ItemManagement.Models;
using OmgvaPOS.ItemVariationManagement.Models;
using OmgvaPOS.TaxManagement.Models;

namespace OmgvaPOS.TaxManagement.Repository;

public class TaxRepository : ITaxRepository
{
    private readonly OmgvaDbContext _database;
    private readonly ILogger<TaxRepository> _logger;

    public TaxRepository(OmgvaDbContext context, ILogger<TaxRepository> logger)
    {
        _database = context;
        _logger = logger;
    }

    public List<Tax> GetAllTaxes()
    {
        return _database.Taxes
            .Where(t => t.IsArchived == false)
            .ToList();
    }

    public Tax? GetTaxById(long id)
    {
        return _database.Taxes
            .FirstOrDefault(tax => tax.Id == id);
    }

    public Tax SaveTax(Tax tax)
    {
        _database.Taxes.Add(tax);
        _database.SaveChanges();
        return tax;
    }

    public Tax UpdateTax(Tax tax)
    {
        var newTax = (Tax)tax.Clone();
        _database.Add(newTax);
        _database.Entry(tax).CurrentValues.SetValues(_database.Entry(tax).OriginalValues);
        tax.IsArchived = true;
        _database.Taxes.Update(tax);
        _database.SaveChanges();
        return newTax;
    }

    public void DeleteTax(long id)
    {
        var tax = _database.Taxes.Find(id);
        if (tax != null)
        {
            tax.IsArchived = true;
            _database.Taxes.Update(tax);
            _database.SaveChanges();
        }
    }
}

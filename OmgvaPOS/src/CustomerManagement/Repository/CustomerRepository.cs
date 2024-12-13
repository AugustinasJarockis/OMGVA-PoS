using OmgvaPOS.CustomerManagement.Models;
using OmgvaPOS.Database.Context;

namespace OmgvaPOS.CustomerManagement.Repository;

public class CustomerRepository : ICustomerRepository
{
    private readonly OmgvaDbContext _context;

    public CustomerRepository(OmgvaDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Customer> GetAll()
    {
        return _context.Customers.ToList();
    }

    public Customer? GetById(long id)
    {
        return _context.Customers.FirstOrDefault(c => c.Id == id);
    }

    public Customer Create(Customer customer)
    {
        _context.Customers.Add(customer);
        _context.SaveChanges();
        return customer;
    }

    public Customer Update(Customer customer)
    {
        _context.Customers.Update(customer);
        _context.SaveChanges();
        return customer;
    }

    public void Delete(long id)
    {
        var customer = _context.Customers.Find(id);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }
    }
}
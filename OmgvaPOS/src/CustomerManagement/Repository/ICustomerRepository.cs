using OmgvaPOS.CustomerManagement.Models;

namespace OmgvaPOS.CustomerManagement.Repository;

public interface ICustomerRepository
{
    IEnumerable<Customer> GetAll();
    Customer? GetById(long id);
    Customer Create(Customer customer);
    Customer Update(Customer customer);
    void Delete(long id);
}

using OmgvaPOS.CustomerManagement.DTOs;

namespace OmgvaPOS.CustomerManagement.Service;

public interface ICustomerService
{
    IEnumerable<CustomerDTO> GetAll();
    CustomerDTO? GetById(long id);
    CustomerDTO Create(CreateCustomerRequest createRequest);
    CustomerDTO Update(long id, UpdateCustomerRequest updateRequest);
    void Delete(long id);
}
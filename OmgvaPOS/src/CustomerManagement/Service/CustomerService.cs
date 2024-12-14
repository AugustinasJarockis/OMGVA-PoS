using OmgvaPOS.CustomerManagement.DTOs;
using OmgvaPOS.CustomerManagement.Mappers;
using OmgvaPOS.CustomerManagement.Repository;
using OmgvaPOS.Exceptions;

namespace OmgvaPOS.CustomerManagement.Service;

public class CustomerService : ICustomerService
{        
    
    private readonly ICustomerRepository _repository;

    public CustomerService(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<CustomerDTO> GetAll()
    {
        var customers = _repository.GetAll();
        return customers.ToDtoList();
    }

    public CustomerDTO? GetById(long id)
    {
        var customer = _repository.GetById(id);
        return customer?.ToDto();
    }

    public CustomerDTO Create(CreateCustomerRequest createRequest)
    {
        var customer = createRequest.ToModel();
            
        var createdCustomer = _repository.Create(customer);
        return createdCustomer.ToDto();
    }

    public CustomerDTO Update(long id, UpdateCustomerRequest updateRequest)
    {
        var existingCustomer = _repository.GetById(id);
        if (existingCustomer == null)
            throw new NotFoundException($"Customer with ID {id} not found");

        existingCustomer.UpdateModel(updateRequest);
            
        var updatedCustomer = _repository.Update(existingCustomer);
        return updatedCustomer.ToDto();
    }

    // TODO think if we really need to throw an exception for DELETE when we cant find by id
    public void Delete(long id)
    {
        var customer = _repository.GetById(id);
        if (customer == null)
            throw new NotFoundException($"Customer with ID {id} not found");

        _repository.Delete(id);
    }
}
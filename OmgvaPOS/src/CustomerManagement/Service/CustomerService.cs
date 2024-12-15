using OmgvaPOS.CustomerManagement.DTOs;
using OmgvaPOS.CustomerManagement.Mappers;
using OmgvaPOS.CustomerManagement.Models;
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
        var existingCustomer = GetCustomerOrThrow(id);

        existingCustomer.UpdateModel(updateRequest);
            
        var updatedCustomer = _repository.Update(existingCustomer);
        return updatedCustomer.ToDto();
    }

    public void Delete(long id)
    {
        var customer = GetCustomerOrThrow(id);

        _repository.Delete(customer.Id);
    }

    private Customer GetCustomerOrThrow(long customerId)
    {
        var customer = _repository.GetById(customerId);
        if (customer == null)
            throw new NotFoundException($"Customer with ID {customerId} not found");

        return customer;
    }
    
}
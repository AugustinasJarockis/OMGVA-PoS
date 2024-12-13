using OmgvaPOS.CustomerManagement.DTOs;
using OmgvaPOS.CustomerManagement.Models;

namespace OmgvaPOS.CustomerManagement.Mappers;

public static class CustomerMapper
{
    public static Customer ToModel(this CreateCustomerRequest createRequest)
    {
        return new Customer()
        {
            Name = createRequest.Name
        };
    }
    
    public static Customer UpdateModel(this Customer customer, UpdateCustomerRequest updateRequest)
    {
        customer.Name = updateRequest.Name ?? customer.Name;
        return customer;
    }

    public static CustomerDTO ToDto(this Customer customer)
    {
        return new CustomerDTO()
        {
            Id = customer.Id,
            Name = customer.Name
        };
    }
    
    public static IEnumerable<CustomerDTO> ToDtoList(this IEnumerable<Customer> customers)
    {
        return customers.Select(ToDto).ToList();
    }
    
}
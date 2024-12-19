using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.CustomerManagement.DTOs;
using OmgvaPOS.CustomerManagement.Service;
using OmgvaPOS.Exceptions;
using OmgvaPOS.HelperUtils;

namespace OmgvaPOS.CustomerManagement.Controller;

[ApiController]
[Route("customers")]
public class CustomerController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<CustomerDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<CustomerDTO> CreateCustomer([FromBody] CreateCustomerRequest createRequest)
    {
        var customer = _customerService.Create(createRequest);
        return CreatedAtAction(nameof(GetCustomer), new { customerId = customer.Id }, customer);
    }

    [HttpGet("{customerId}")]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<CustomerDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<CustomerDTO> GetCustomer(long customerId)
    {
        var customer = _customerService.GetById(customerId);
        if (customer == null)
            return NotFound($"Customer with ID {customerId} not found");
        
        return Ok(customer);
    }
    
    [HttpPut("{customerId}")]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<CustomerDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<CustomerDTO> UpdateCustomer(long customerId, [FromBody] UpdateCustomerRequest updateRequest)
    {
        var customer = _customerService.Update(customerId, updateRequest);
        return Ok(customer);
    }

    [HttpDelete("{customerId}")]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<CustomerDTO>(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult DeleteCustomer(long customerId)
    {
        _customerService.Delete(customerId);
        return NoContent();
    }

    [HttpGet("business/{businessId}")]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<CustomerDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<CustomerDTO>> GetBusinessCustomers(long businessId)
    {
        if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, businessId))
            throw new ForbiddenException("Forbidden");
        
        var customers = _customerService.GetAll();
        return Ok(customers);
    }
}
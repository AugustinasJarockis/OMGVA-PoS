using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.TaxManagement.DTOs;
using OmgvaPOS.TaxManagement.Mappers;
using OmgvaPOS.TaxManagement.Models;
using OmgvaPOS.TaxManagement.Services;
using System.Linq;

namespace OmgvaPOS.TaxManagement.Controller
{
    [ApiController]
    [Route("tax")]
    public class TaxController : ControllerBase
    {
        private readonly ITaxService _taxService;
        private readonly ILogger<TaxController> _logger;

        public TaxController(ITaxService taxService, ILogger<TaxController> logger)
        {
            _taxService = taxService;
            _logger = logger;
        }
        
        private const string TaxNotFoundMessage = "Tax not found.";

        [HttpPost]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType<TaxDto>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTax([FromBody] CreateTaxRequest createTaxRequest)
        {
            var tax = TaxMapper.FromCreateTaxRequest(createTaxRequest);
            _taxService.CreateTax(tax);

            return CreatedAtAction(nameof(GetTaxById), new { tax.Id }, tax);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType<TaxDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTax([FromBody] UpdateTaxRequest updateTaxRequest, long id)
        {
            var tax = _taxService.GetTaxById(id);
            if (tax == null)
                return NotFound(TaxNotFoundMessage);
            
            tax = TaxMapper.FromUpdateTaxRequest(updateTaxRequest, tax);
            var returnTax = _taxService.UpdateTax(tax);
            return Ok(returnTax);
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<List<TaxDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllTaxes()
        {
            var taxes = _taxService.GetAllTaxes();
            return Ok(TaxMapper.ToDTOs(taxes));
        }
        
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetTaxById(long id)
        {
            var tax = _taxService.GetTaxById(id);
            if (tax == null)
                return NotFound(TaxNotFoundMessage);

            return Ok(TaxMapper.ToDTO(tax));
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTaxById(long id)
        {
            var tax = _taxService.GetTaxById(id);
            if (tax == null)
                return NotFound(TaxNotFoundMessage);
            
            _taxService.DeleteTax(id);
            return NoContent();
        }
    }
}
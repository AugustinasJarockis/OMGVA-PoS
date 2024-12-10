using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.TaxManagement.DTOs;
using OmgvaPOS.TaxManagement.Mappers;
using OmgvaPOS.TaxManagement.Models;
using OmgvaPOS.TaxManagement.Repository;

namespace OmgvaPOS.TaxManagement.Controller
{
    [ApiController]
    [Route("tax")]
    public class TaxController : ControllerBase
    {
        private readonly ITaxRepository _taxRepository;
        private readonly ILogger<TaxController> _logger;

        public TaxController(ITaxRepository taxRepository, ILogger<TaxController> logger)
        {
            _taxRepository = taxRepository;
            _logger = logger;
        }
        
        private const string TaxNotFoundMessage = "Tax not found.";

        [HttpPost]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType<TaxDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTax([FromBody] CreateTaxRequest createTaxRequest)
        {
            var tax = TaxMapper.FromCreateTaxRequest(createTaxRequest);
            await _taxRepository.SaveTaxAsync(tax);

            return CreatedAtAction(nameof(GetTaxById), new { tax.Id }, tax);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTax([FromBody] UpdateTaxRequest updateTaxRequest, long id)
        {
            var tax = await _taxRepository.GetTaxByIdAsync(id);
            if (tax == null)
            {
                return NotFound(TaxNotFoundMessage);
            }
            
            tax = TaxMapper.FromUpdateTaxRequest(updateTaxRequest, tax);
            await _taxRepository.UpdateTaxAsync(tax);
            return NoContent();
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<List<TaxDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TaxDto>>> GetAllTaxes()
        {
            var taxes = await _taxRepository.GetAllTaxesAsync();
            return Ok(TaxMapper.ToDTOs(taxes));
        }
        
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TaxDto>> GetTaxById(long id)
        {
            var tax = await _taxRepository.GetTaxByIdAsync(id);
            if (tax == null)
            {
                return NotFound(TaxNotFoundMessage);
            }

            return Ok(TaxMapper.ToDTO(tax));
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteTaxById(long id)
        {
            var tax = await _taxRepository.GetTaxByIdAsync(id);
            if (tax == null)
            {
                return NotFound(TaxNotFoundMessage);
            }
            await _taxRepository.DeleteTaxAsync(id);
            return NoContent();
        }
    }
}
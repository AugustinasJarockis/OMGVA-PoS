using Microsoft.AspNetCore.Mvc;
using OMGVA_PoS.Data_layer.DTOs;
using OMGVA_PoS.Data_layer.Mappers;
using OMGVA_PoS.Data_layer.Repositories.Tax;

namespace OMGVA_PoS.Business_layer.Controllers
{
    [ApiController]
    [Route("tax")]
    public class TaxController(ITaxRepository taxRepository) : ControllerBase
    {

        private static string TaxNotFound(long id) => $"Tax with ID {id} not found.";

        [HttpPost]
        public async Task<IActionResult> CreateTax([FromBody] CreateTaxRequest createTaxRequest)
        {
            var tax = TaxMapper.FromCreateTaxRequest(createTaxRequest);
            await taxRepository.SaveTaxAsync(tax);

            return CreatedAtAction(nameof(GetTaxById), new { tax.Id }, tax);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTax([FromBody] UpdateTaxRequest updateTaxRequest, long id)
        {
            var tax = await taxRepository.GetTaxByIdAsync(id);
            if (tax == null)
            {
                return NotFound(TaxNotFound(id));
            }
            
            tax = TaxMapper.FromUpdateTaxRequest(updateTaxRequest, tax);
            await taxRepository.UpdateTaxAsync(tax);
            return NoContent();
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaxDTO>>> GetAllTaxes()
        {
            var taxes = await taxRepository.GetAllTaxesAsync();
            return Ok(TaxMapper.ToDTOs(taxes));
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<TaxDTO>> GetTaxById(long id)
        {
            var tax = await taxRepository.GetTaxByIdAsync(id);

            if (tax == null)
            {
                return NotFound(TaxNotFound(id));
            }

            return Ok(TaxMapper.ToDTO(tax));
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTaxById(long id)
        {
            await taxRepository.DeleteTaxAsync(id);
            return NoContent();
        }
    }
}
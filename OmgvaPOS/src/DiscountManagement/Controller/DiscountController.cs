using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.HelperUtils;
using OmgvaPOS.DiscountManagement.DTOs;
using OmgvaPOS.DiscountManagement.Mappers;
using OmgvaPOS.DiscountManagement.Service;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace OmgvaPOS.DiscountManagement.Controller
{

    [ApiController]
    [Route("discount")]

    public class DiscountController(IDiscountService discountService, ILogger<DiscountController> logger) : ControllerBase {

        private readonly IDiscountService _discountService = discountService;
        private readonly ILogger<DiscountController> _logger = logger;
        //TODO: fix error handling once we get exception middleware
        // hopefully with our own exceptions
        // since I'd like to return correct HTTP response methods.
        [HttpPost]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType<DiscountDTO>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateDiscount([FromBody] CreateDiscountRequest createDiscountRequest) {
            long? businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization!);
            if (businessId == null)
                return Forbid();
            else
                createDiscountRequest.BusinessId = businessId;

            try {
                var discount = _discountService.CreateDiscount(createDiscountRequest);
                DiscountDTO discountDTO = DiscountMapper.ToDTO(discount);
                return CreatedAtAction(nameof(GetDiscountById), new { discountDTO.Id }, discountDTO);
            }
            catch (ValidationException) {
                return StatusCode((int)HttpStatusCode.UnprocessableEntity, "New discount has wrong fields");
            }
            catch (NotImplementedException ex) {
                return NotFound(ex.Message);
            }
            catch (ApplicationException) {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
            catch { throw; }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<List<DiscountDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<DiscountDTO>> GetAllDiscounts() {
            long? businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization!);
            if (businessId == null) return Forbid();

            try {
                List<DiscountDTO> discountDTOs;

                if (HttpContext.User.IsInRole("Admin"))
                    discountDTOs = _discountService.GetGlobalDiscounts();
                else
                    discountDTOs = _discountService.GetBusinessDiscounts((long)businessId);

                return Ok(discountDTOs);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unexpected internal server error occured while getting all discounts.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<DiscountDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DiscountDTO> GetDiscountById(long id) {
            if (!JwtTokenHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _discountService.GetDiscountNoException(id).BusinessId))
                return Forbid();

            try {
                DiscountDTO discountDTO = _discountService.GetDiscountById(id);
                return Ok(discountDTO);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unexpected internal server error occured while trying to get a specific discount.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateDiscountValidUntil([FromBody] DateTime newValidUntil, long id) {
            if (!JwtTokenHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _discountService.GetDiscountNoException(id).BusinessId))
                return Forbid();
            // there is really nothing else you can update in a discount
            // since we care about historical data:
            // changing discount% or discount type would require creating a new discount all together
            try {
                _discountService.UpdateDiscountValidUntil(id, newValidUntil);
                return NoContent();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unexpected internal server error occured while updating the discount.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ArchiveDiscount(long id) {
            if (!JwtTokenHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _discountService.GetDiscountNoException(id).BusinessId))
                return Forbid();

            // essentially update IsArchived to true
            try {
                _discountService.ArchiveDiscount(id);
                return NoContent();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unexpected internal server error occured while updating the discount.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("{discountId}/item/{itemId}")]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //TODO: does not work error in ItemRepository UpdateItem(Item item)
        public IActionResult UpdateDiscountOfItem(long discountId, long itemId) {
            if (!JwtTokenHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _discountService.GetDiscountNoException(discountId).BusinessId))
                return Forbid();

            try {
                _discountService.UpdateDiscountOfItem(discountId, itemId);
                return NoContent();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unexpected internal server error occured while updating the discount.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

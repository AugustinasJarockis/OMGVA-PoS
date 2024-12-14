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
            long businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization!);
            createDiscountRequest.BusinessId = businessId;

            var discountDTO = _discountService.CreateDiscount(createDiscountRequest);
            return CreatedAtAction(nameof(GetDiscountById), new { discountDTO.Id }, discountDTO);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<List<DiscountDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<DiscountDTO>> GetAllDiscounts() {
            long businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization!);

            var discountDTOs = _discountService.GetBusinessDiscounts((long)businessId);

            return Ok(discountDTOs);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<DiscountDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DiscountDTO> GetDiscountById(long id) {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _discountService.GetDiscountModel(id).BusinessId))
                return Forbid();

            var discountDTO = _discountService.GetDiscountById(id);
            return Ok(discountDTO);
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
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _discountService.GetDiscountModel(id).BusinessId))
                return Forbid();
            // TODO there is really nothing else you can update in a discount
            // since we care about historical data:
            // changing discount% or discount type would require creating a new discount all together
            _discountService.UpdateDiscountValidUntil(id, newValidUntil);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ArchiveDiscount(long id) {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _discountService.GetDiscountModel(id).BusinessId))
                return Forbid();

            // essentially update IsArchived to true
            _discountService.ArchiveDiscount(id);
            return NoContent();
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
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _discountService.GetDiscountModel(discountId).BusinessId))
                return Forbid();

            _discountService.UpdateDiscountOfItem(discountId, itemId);
            return NoContent();
        }
    }
}

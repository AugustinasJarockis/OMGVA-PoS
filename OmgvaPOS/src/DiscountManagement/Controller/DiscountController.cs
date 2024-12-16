using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.HelperUtils;
using OmgvaPOS.DiscountManagement.DTOs;
using OmgvaPOS.DiscountManagement.Service;
using OmgvaPOS.OrderManagement.Service;

namespace OmgvaPOS.DiscountManagement.Controller
{
    [ApiController]
    [Route("discount")]

    public class DiscountController(IDiscountService discountService, IOrderService orderService, ILogger<DiscountController> logger) : ControllerBase {

        private readonly IDiscountService _discountService = discountService;
        private readonly IOrderService _orderService = orderService;
        private readonly ILogger<DiscountController> _logger = logger;

        [HttpPost]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType<DiscountDTO>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateDiscount([FromBody] CreateDiscountRequest createDiscountRequest) {
            var businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization!);
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

            var discountDTOs = _discountService.GetBusinessDiscounts(businessId);

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
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _discountService.GetDiscountBusinessId(id)))
                return Forbid();
            
            return Ok(_discountService.GetDiscountById(id));
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateDiscountValidUntil([FromBody] DateTime newValidUntil, long id) {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _discountService.GetDiscountBusinessId(id)))
                return Forbid();

            _discountService.UpdateDiscountValidUntil(id, newValidUntil);
            return NoContent();
        }

        [HttpPatch("{discountId}/order/{orderId}")]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateOrderDiscountAmount([FromBody] short amount, long discountId, long orderId) {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _discountService.GetDiscountBusinessId(discountId)))
                return Forbid();
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _orderService.GetOrderBusinessId(discountId)))
                return Forbid();

            _discountService.UpdateOrderDiscountAmount(discountId, orderId, amount);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ArchiveDiscount(long id) {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _discountService.GetDiscountBusinessId(id)))
                return Forbid();

            // essentially update IsArchived to true
            _discountService.ArchiveDiscount(id);
            return NoContent();
        }
    }
}

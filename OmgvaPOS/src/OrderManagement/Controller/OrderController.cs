using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.DiscountManagement.Controller;
using OmgvaPOS.HelperUtils;
using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderManagement.Service;

namespace OmgvaPOS.OrderManagement.Controller;


[ApiController]
[Route("order")]
public class OrderController(IOrderService orderService, ILogger<DiscountController> logger) : ControllerBase
{

    private readonly IOrderService _orderService = orderService;
    private readonly ILogger<DiscountController> _logger = logger;

    [HttpPost]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<OrderDTO>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<OrderDTO> CreateOrder([FromBody] CreateOrderRequest request) {
        long? businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization!);
        long? userId = JwtTokenHandler.GetTokenUserId(HttpContext.Request.Headers.Authorization!);
        if (businessId == null || userId == null)
            return Forbid();

        request.BusinessId = businessId;
        request.UserId = userId;

        try {
            var orderDTO = _orderService.CreateOrder(request);
            return CreatedAtAction(nameof(GetOrderById), new { orderDTO.Id }, orderDTO);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An unexpected internal server error occured while trying to create an order.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<OrderDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<OrderDTO> GetOrderById(long id) {
        if (!JwtTokenHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _orderService.GetOrderBusinessId(id)))
            return Forbid();

        try {
            OrderDTO orderDTO = _orderService.GetOrder(id);
            return Ok(orderDTO);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An unexpected internal server error occured while trying to get an order.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<OrderDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<OrderDTO>> GetAllBusinessOrders() {
        long? businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization!);
        if (businessId == null) return Forbid();

        try {
            var orderDTOs = _orderService.GetAllBusinessOrders((long)businessId);

            return Ok(orderDTOs);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An unexpected internal server error occured while getting all orders.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("active")]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<OrderDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<OrderDTO>> GetAllActiveOrders() {
        long? businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization!);
        if (businessId == null) return Forbid();

        try {
            var orderDTOs = _orderService.GetAllActiveOrders((long)businessId);

            return Ok(orderDTOs);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An unexpected internal server error occured while getting all active orders.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<OrderDTO>(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateActiveOrder([FromBody] OrderDTO orderDTO, long id) {
        if (!JwtTokenHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _orderService.GetOrderBusinessId(id)))
            return Forbid();
        if (orderDTO == null)
            return BadRequest("Include how you want to update it.");

        try {
            _orderService.UpdateOrder(orderDTO, id);
            return NoContent();
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An unexpected internal server error occured while updating the order.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}

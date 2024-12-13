using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.DiscountManagement.Controller;
using OmgvaPOS.HelperUtils;
using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderManagement.Service;

namespace OmgvaPOS.OrderManagement.Controller;

[ApiController]
[Route("order")]
public class OrderUpdateController(IOrderService orderService, ILogger<DiscountController> logger) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;
    private readonly ILogger<DiscountController> _logger = logger;

    [HttpDelete("{orderId}/item/{itemId}")]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<OrderDTO>(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteOrderItem(long orderId, long itemId) {
        if (!JwtTokenHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _orderService.GetOrderBusinessId(orderId)))
            return Forbid();

        try {
            _orderService.DeleteOrderItem(orderId, itemId);
            return NoContent();
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An unexpected internal server error occured while deleting order item.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost("{orderId}/item")]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<OrderDTO>(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult AddOrderItem([FromBody] CreateOrderItemRequest request, long orderId) {
        if (!JwtTokenHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _orderService.GetOrderBusinessId(orderId)))
            return Forbid();

        try {
            _orderService.AddOrderItem(orderId, request);
            return NoContent();
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An unexpected internal server error occured while deleting order item.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPatch("{orderId}/item/{itemId}")]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<OrderDTO>(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateOrderItem([FromBody] UpdateOrderItemRequest request, long orderId, long itemId) {
        if (!JwtTokenHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _orderService.GetOrderBusinessId(orderId)))
            return Forbid();

        try {
            _orderService.UpdateOrderItem(itemId, request);
            return NoContent();
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An unexpected internal server error occured while deleting order item.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.DiscountManagement.Controller;
using OmgvaPOS.HelperUtils;
using OmgvaPOS.OrderItemManagement.Service;
using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderManagement.Service;
using OmgvaPOS.OrderItemManagement.DTOs;

namespace OmgvaPOS.OrderManagement.Controller;

[ApiController]
[Route("order/{orderId}/item")]
public class OrderItemController(IOrderService orderService, IOrderItemService orderItemService, ILogger<DiscountController> logger) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;
    private readonly IOrderItemService _orderItemService = orderItemService;
    private readonly ILogger<DiscountController> _logger = logger;

    [HttpPost]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<OrderDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<OrderItemDTO> AddOrderItem([FromBody] CreateOrderItemRequest request, long orderId) {
        if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _orderService.GetOrderBusinessId(orderId)))
            return Forbid();

        var orderItemDTO = _orderItemService.AddOrderItem(orderId, request);
        return Ok(orderItemDTO);
    }

    [HttpGet("{orderItemId}")]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<OrderItemDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetOrderItem(long orderId, long orderItemId) {
        if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _orderService.GetOrderBusinessId(orderId)))
            return Forbid();

        return Ok(_orderItemService.GetOrderItem(orderItemId));
    }

    [HttpPatch("{orderItemId}")]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateOrderItem([FromBody] UpdateOrderItemRequest request, long orderId, long orderItemId) {
        if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _orderService.GetOrderBusinessId(orderId)))
            return Forbid();

        _orderItemService.UpdateOrderItem(orderItemId, request);
        return NoContent();
    }


    [HttpDelete("{orderItemId}")]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteOrderItem(long orderId, long orderItemId) {
        if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _orderService.GetOrderBusinessId(orderId)))
            return Forbid();

        _orderItemService.DeleteOrderItem(orderItemId);
        return NoContent();
    }
}

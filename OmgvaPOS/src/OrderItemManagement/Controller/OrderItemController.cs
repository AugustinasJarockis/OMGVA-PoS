using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.HelperUtils;
using OmgvaPOS.OrderItemManagement.DTOs;
using OmgvaPOS.OrderItemManagement.Service;
using OmgvaPOS.OrderManagement.Service;

namespace OmgvaPOS.OrderItemManagement.Controller;

[ApiController]
[Route("order/{orderId}/item")]
public class OrderItemController(IOrderService orderService, IOrderItemService orderItemService) : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly IOrderService _orderService = orderService;
    private readonly IOrderItemService _orderItemService = orderItemService;

    [HttpPost]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult AddOrderItem([FromBody] CreateOrderItemRequest request, long orderId) {
        if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _orderService.GetOrderBusinessId(orderId)))
            return Forbid();

        _orderItemService.AddOrderItem(orderId, request);
        return NoContent();
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

        _orderItemService.DeleteOrderItem(orderItemId, true);
        return NoContent();
    }
}

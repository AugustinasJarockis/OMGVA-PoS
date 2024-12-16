﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.DiscountManagement.Controller;
using OmgvaPOS.HelperUtils;
using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderManagement.Service;

namespace OmgvaPOS.OrderManagement.Controller;


[ApiController]
[Route("order")]
public class OrderController(IOrderService orderService, ILogger<DiscountController> logger) : Microsoft.AspNetCore.Mvc.Controller
{

    private readonly IOrderService _orderService = orderService;
    private readonly ILogger<DiscountController> _logger = logger;

    [HttpPost]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<OrderDTO>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<OrderDTO> CreateOrder() {
        long? businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization!);
        long? userId = JwtTokenHandler.GetTokenUserId(HttpContext.Request.Headers.Authorization!);
        if (businessId == null || userId == null)
            return Forbid();

        var orderDTO = _orderService.CreateOrder((long)businessId, (long)userId);

        return CreatedAtAction(nameof(GetOrderById), new { orderId = orderDTO.Id }, orderDTO);
    }

    [HttpGet("{orderId}")]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<OrderDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<OrderDTO> GetOrderById(long orderId) {
        if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _orderService.GetOrderBusinessId(orderId)))
            return Forbid();

        OrderDTO orderDTO = _orderService.GetOrder(orderId);
        return Ok(orderDTO);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<IEnumerable<OrderDTO>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<OrderDTO>> GetAllBusinessOrders() {
        long? businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization!);
        if (businessId == null) return Forbid();


        var orderDTOs = _orderService.GetAllBusinessOrders((long)businessId);

        return Ok(orderDTOs);
    }

    [HttpGet("active")]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<OrderDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<OrderDTO>> GetAllActiveOrders() {
        long? businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization!);
        if (businessId == null) return Forbid();

        var orderDTOs = _orderService.GetAllActiveOrders((long)businessId);
        return Ok(orderDTOs);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<OrderDTO>(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteActiveOrder(long id) {
        if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _orderService.GetOrderBusinessId(id)))
            return Forbid();

        _orderService.DeleteOrder(id);
        return NoContent();
    }

    [HttpPatch("tip/{orderId}")]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<OrderDTO>(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateOrderTip(short tip, long orderId) {
        if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _orderService.GetOrderBusinessId(orderId)))
            return Forbid();

        _orderService.UpdateOrderTip(tip, orderId);
        return NoContent();
    }
}
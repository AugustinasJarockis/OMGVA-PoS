﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.DiscountManagement.Controller;
using OmgvaPOS.HelperUtils;
using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderManagement.Enums;
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
    public ActionResult<IEnumerable<OrderDTO>> GetAllBusinessOrders([FromQuery] OrderStatus? orderStatus, 
                                                                    [FromQuery] int? pageSize, 
                                                                    [FromQuery] int? page) { 
        
        var businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization!);
        var queryParams = new OrdersRequestCriteria(orderStatus, page, pageSize);
        
        var (orderDTOs, totalPagesCount) = _orderService.GetBusinessOrdersWithRequestCriteria(businessId, queryParams);

        Response.Headers.Append("totalPagesCount", totalPagesCount.ToString());
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

    [HttpDelete("{orderId}")]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<OrderDTO>(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteActiveOrder(long orderId) {
        if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _orderService.GetOrderBusinessId(orderId)))
            return Forbid();

        _orderService.DeleteOrder(orderId);
        return NoContent();
    }

    [HttpPatch("{orderId}")]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<OrderDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<OrderDTO> UpdateOrder([FromBody] UpdateOrderRequest updateRequest, long orderId) 
    {
        if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization, _orderService.GetOrderBusinessId(orderId)))
            return Forbid();

        var updatedOrderDTO = _orderService.UpdateOrder(updateRequest, orderId);
        return Ok(updatedOrderDTO);
    }

    [HttpPost("{orderId}/split")]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<List<SimpleOrderDTO>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult SplitOrder(long orderId, [FromBody] SplitOrderRequest splitOrderRequest) {
        if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _orderService.GetOrderBusinessId(orderId)))
            return Forbid();

        var simpleOrderDTOs = _orderService.SplitOrder(orderId, splitOrderRequest);
        
        return Ok(simpleOrderDTOs);
    }

    [HttpPost("{orderId}/refund")]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<OrderDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<OrderDTO> RefundOrder([FromBody] RefundOrderRequest refundOrderRequest, long orderId) {
        if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _orderService.GetOrderBusinessId(orderId)))
            return Forbid();

        _orderService.RefundOrder(refundOrderRequest, orderId);
        OrderDTO orderDTO = _orderService.GetOrder(orderId);
        return Ok(orderDTO);
    }

    [HttpPost("{orderId}/cancel")]
    [Authorize(Roles = "Admin,Owner,Employee")]
    [ProducesResponseType<OrderDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<OrderDTO> CancelOrder(long orderId) {
        if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _orderService.GetOrderBusinessId(orderId)))
            return Forbid();

        _orderService.CancelOrder(orderId);
        OrderDTO orderDTO = _orderService.GetOrder(orderId);
        return Ok(orderDTO);
    }
}

using OmgvaPOS.DiscountManagement.Mappers;
using OmgvaPOS.DiscountManagement.Models;
using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderManagement.Mappers;
using OmgvaPOS.OrderManagement.Repository;

namespace OmgvaPOS.OrderManagement.Service;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrderService> _logger;
    public OrderService(
        IOrderRepository orderRepository,
        ILogger<OrderService> logger
        ) {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public OrderDTO CreateOrder(CreateOrderRequest request) {
        if (request.CreateOrderItemRequests == null || !request.CreateOrderItemRequests.Any())
            throw new ArgumentException("Order must have at least one item.");

        var order = OrderMappers.RequestToOrder(request);
        order = _orderRepository.AddOrder(order);

        return OrderMappers.OrderToOrderDTO(order);
    }
}

using OmgvaPOS.Exceptions;
using OmgvaPOS.OrderManagement.Enums;
using OmgvaPOS.OrderManagement.Models;

namespace OmgvaPOS.OrderManagement.Validators;

public static class OrderValidator
{
    public static void Exists(Order order) {
        if (order == null)
            throw new NotFoundException("Order doesn't exist/not found");
    }

    public static void Exist(IEnumerable<object> orders) {
        if (orders == null || !orders.Any())
            throw new NotFoundException("No Orders for business found");
    }

    public static void IsOpen(Order order) {
        if (order.Status != OrderStatus.Open)
            throw new ValidationException($"Order with ID {order.Id} is {order.Status} (not open for editing)");
    }
}

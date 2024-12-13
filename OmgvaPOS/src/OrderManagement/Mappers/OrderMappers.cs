using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderManagement.Enums;
using OmgvaPOS.OrderManagement.Models;
using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderItemVariationManagement.Models;
using OmgvaPOS.OrderItemManagement.DTOs;
using OmgvaPOS.OrderItemVariationManagement.DTOs;

namespace OmgvaPOS.OrderManagement.Mappers
{
    public static class OrderMappers
    {
        public static Order RequestToOrder(this CreateOrderRequest request) {
            return new Order {
                Status = OrderStatus.Open,
                Tip = 0,
                UserId = (long)request.UserId,
                OrderItems = request.CreateOrderItemRequests
                    .Select(createOrderItemRequest => RequestToOrderItem(createOrderItemRequest))
                    .ToList()
            };
        }

        // No, I will not separate these into different files.
        public static OrderItem RequestToOrderItem(this CreateOrderItemRequest request) {
            return new OrderItem {
                ItemId = request.ItemId,
                Quantity = request.Quantity,
                OrderItemVariation = request.CreateOrderItemVariationRequest == null
                    ? null
                    : RequestToOrderItemVariation(request.CreateOrderItemVariationRequest)
            };
        }

        public static OrderItemVariation RequestToOrderItemVariation(this CreateOrderItemVariationRequest request) {
            return new OrderItemVariation {
                ItemVariationId = request.ItemVariationId
            };
        }

        public static OrderDTO OrderToOrderDTO(this Order order) {
            return new OrderDTO {
                Id = order.Id,
                Status = order.Status,
                Tip = order.Tip,
                RefundReason = order.RefundReason,
                UserId = order.UserId,
                DiscountId = order.DiscountId,
                OrderItems = order.OrderItems.Select(orderItem => new OrderItemDTO {
                    Id = orderItem.Id,
                    ItemId = orderItem.ItemId,
                    Quantity = orderItem.Quantity,
                    DiscountId = orderItem.DiscountId,
                    OrderItemVariation = orderItem.OrderItemVariation == null ? null : new OrderItemVariationDTO {
                        Id = orderItem.OrderItemVariation.Id,
                        ItemVariationId = orderItem.OrderItemVariation.ItemVariationId
                    }
                }).ToList()
            };
        }
    }
}

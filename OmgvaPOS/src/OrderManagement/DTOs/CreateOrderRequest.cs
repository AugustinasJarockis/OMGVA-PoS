namespace OmgvaPOS.OrderManagement.DTOs
{
    public class CreateOrderRequest
    {
        // according to specification:
        // creating an order is only done after selecting the items
        // so this request mainly contains the items themselves.
        public long? UserId { get; set; }
        public long? BusinessId { get; set; }
        public required ICollection<CreateOrderItemRequest> CreateOrderItemRequests { get; set; }
    }

    public class CreateOrderItemRequest
    {
        public short Quantity { get; set; }
        public long ItemId { get; set; }
        public CreateOrderItemVariationRequest? CreateOrderItemVariationRequest { get; set; }
    }

    public class CreateOrderItemVariationRequest
    {
        public long ItemVariationId { get; set; }
    }
}

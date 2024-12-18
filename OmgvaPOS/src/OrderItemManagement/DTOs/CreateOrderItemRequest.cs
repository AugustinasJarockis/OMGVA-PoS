namespace OmgvaPOS.OrderItemManagement.DTOs;

public class CreateOrderItemRequest
{
    public short Quantity { get; set; }
    public long ItemId { get; set; }
    public ICollection<long>? ItemVariationIds { get; set; }
}

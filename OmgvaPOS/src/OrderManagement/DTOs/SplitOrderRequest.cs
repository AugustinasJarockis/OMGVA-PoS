namespace OmgvaPOS.OrderManagement.DTOs
{
    public class SplitOrderItem
    {
        public long OrderItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class SplitOrderRequest
    {
        public List<SplitOrderItem> SplitOrderItems { get; set; }
    }
}

namespace OMGVA_PoS.Data_layer.Models
{
    public class OrderItem
    {
        public long Id { get; set; }
        public short Quantity { get; set; }
        public long ItemId { get; set; }
        public long OrderId { get; set; }
        public long? DiscountId { get; set; }  
    }
}

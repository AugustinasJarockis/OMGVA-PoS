using OmgvaPOS.OrderManagement.Enums;
using OmgvaPOS.TaxManagement.Models;
using OmgvaPOS.UserManagement.Models;

namespace OmgvaPOS.OrderManagement.DTOs
{
    public class OrderDTO
    {
        public long Id { get; set; }
        public OrderStatus Status { get; set; }
        public decimal FinalPrice { get; set; }
        public string? Currency { get; set; }
        public decimal TaxesPaid { get; set; }
        public decimal Tip { get; set; }
        public string? RefundReason { get; set; }
        public SimpleDiscountDTO? Discount { get; set; }
        public SimpleUserDTO User { get; set; }
        public IEnumerable<OrderItemDTO> OrderItems { get; set; }
    }
    public class OrderItemDTO
    {
        public long Id { get; set; }
        public decimal TotalPrice { get; set; }
        public string Currency { get; set; }
        public decimal UnitPriceNoDiscount { get; set; }
        public short TaxPercent { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public short Quantity { get; set; }
        public short MaxQuantity { get; set; }
        public SimpleDiscountDTO? Discount { get; set; }
        public IEnumerable<OrderItemVariationDTO>? Variations { get; set; }
    }
    public class OrderItemVariationDTO
    {
        public long Id { get; set; }
        public long ItemVariationId { get; set; }
        public string ItemVariationName { get; set; }
        public string ItemVariationGroup { get; set; }
        public decimal PriceChange { get; set; }
    }
}

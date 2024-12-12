using OmgvaPOS.DiscountManagement.Enums;

namespace OmgvaPOS.DiscountManagement.DTOs;
public class DiscountDTO {

    public long Id { get; set; }
    public short Amount { get; set; }
    public DateTime TimeValidUntil { get; set; }
    public DiscountType Type { get; set; }
    public bool IsArchived { get; set; }

}

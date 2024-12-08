namespace OMGVA_PoS.Data_layer.Enums
{
    public enum DiscountType {
        Item,
        Order
    }
    
    public enum OrderStatus {
        Open,
        Closed,
        Cancelled,
        Refunded
    }

    public enum UserRole {
        Employee,
        Owner,
        Admin
    }

    public enum PaymentMethod {
        Cash,
        Card,
        Giftcard
    }

    public enum ReservationStatus {
        Open,
        Cancelled,
        Done
    }
}

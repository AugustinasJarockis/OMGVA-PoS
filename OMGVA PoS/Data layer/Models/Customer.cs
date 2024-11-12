namespace OMGVA_PoS.Data_layer.Models
{
    public class Customer
    {
        public long Id { get; set; }
        public string Name { get; set; }

        // navigational properties
        public ICollection<Payment> Payments { get; set; } // customer can pay
        public ICollection<Reservation> Reservations { get; set; } // customer can make reservations
    }
}

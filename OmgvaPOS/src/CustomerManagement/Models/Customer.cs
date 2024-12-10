using OmgvaPOS.PaymentManagement.Models;
using OmgvaPOS.ReservationManagement.Models;

namespace OmgvaPOS.CustomerManagement.Models
{
    public class Customer
    {
        public long Id { get; set; }
        public string Name { get; set; }

        // navigational properties
        public ICollection<PaymentManagement.Models.Payment> Payments { get; set; } // customer can pay
        public ICollection<Reservation> Reservations { get; set; } // customer can make reservations
    }
}

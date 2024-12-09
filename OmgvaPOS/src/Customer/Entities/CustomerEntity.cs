using OmgvaPOS.Payment.Entities;
using OmgvaPOS.Reservation.Entities;

namespace OmgvaPOS.Customer.Entities
{
    public class CustomerEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }

        // navigational properties
        public ICollection<PaymentEntity> Payments { get; set; } // customer can pay
        public ICollection<ReservationEntity> Reservations { get; set; } // customer can make reservations
    }
}

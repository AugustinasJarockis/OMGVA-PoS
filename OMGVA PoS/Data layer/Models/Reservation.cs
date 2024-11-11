using OMGVA_PoS.Data_layer.Enums;

namespace OMGVA_PoS.Data_layer.Models
{
    public class Reservation
    {
        public long Id { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeReserved { get; set; }
        public ReservationStatus Status { get; set; }
        public long EmployeeId { get; set; }
        public long CustomerId { get; set; }

    }
}

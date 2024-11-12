using OMGVA_PoS.Data_layer.Enums;
using System.ComponentModel.DataAnnotations.Schema;

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

        // navigational properties
        // for foreign keys
        [ForeignKey(nameof(EmployeeId))] // explicit since not called UserId
        public User User { get; set; }
        public Customer Customer { get; set; }
    }
}

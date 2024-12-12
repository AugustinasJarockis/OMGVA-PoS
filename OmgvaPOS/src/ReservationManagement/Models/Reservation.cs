using System.ComponentModel.DataAnnotations.Schema;
using OmgvaPOS.CustomerManagement.Models;
using OmgvaPOS.ReservationManagement.Enums;
using OmgvaPOS.UserManagement.Models;

namespace OmgvaPOS.ReservationManagement.Models
{
    // TODO: add item to link what service will be provided with this Reservation
    // TODO: add business or reuse business from item to protect this resource
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

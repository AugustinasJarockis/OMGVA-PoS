using System.ComponentModel.DataAnnotations.Schema;
using OmgvaPOS.Customer.Entities;
using OmgvaPOS.Reservation.Enums;
using OmgvaPOS.UserManagement.Entities;

namespace OmgvaPOS.Reservation.Entities
{
    public class ReservationEntity
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
        public UserEntity UserEntity { get; set; }
        public CustomerEntity CustomerEntity { get; set; }
    }
}

using OmgvaPOS.BusinessManagement.Entities;
using OmgvaPOS.Schedule.Entities;
using OmgvaPOS.UserManagement.Enums;

namespace OmgvaPOS.UserManagement.Entities
{
    public class UserEntity
    {
        public long Id { get; set; }
        public long? BusinessId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public string Password { get; set; }
        public bool HasLeft { get; set; }

        // navigational properties
        public ICollection<Order.Entities.OrderEntity> Orders { get; set; } // User can do Orders
        public ICollection<Reservation.Entities.ReservationEntity> Reservations { get; set; } // User (employee) can have reservations
        public ICollection<EmployeeScheduleEntity> EmployeeSchedules { get; set; } // several since EmployeeScehdule is for one day only
        
        // for foreign keys
        public BusinessEntity BusinessEntity { get; set; }

    }
}

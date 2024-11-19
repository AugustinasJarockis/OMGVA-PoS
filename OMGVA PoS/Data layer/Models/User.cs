using OMGVA_PoS.Data_layer.Enums;

namespace OMGVA_PoS.Data_layer.Models
{
    public class User
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
        public ICollection<Order> Orders { get; set; } // User can do Orders
        public ICollection<Reservation> Reservations { get; set; } // User (employee) can have reservations
        public ICollection<EmployeeSchedule> EmployeeSchedules { get; set; } // several since EmployeeScehdule is for one day only
        
        // for foreign keys
        public Business Business { get; set; }

    }
}

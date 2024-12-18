using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using OmgvaPOS.UserManagement.Models;

namespace OmgvaPOS.ScheduleManagement.Models
{
    public class EmployeeSchedule
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public DateOnly Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsCancelled { get; set; }

        // navigational properties
        // for foreign keys
        [JsonIgnore]
        [ForeignKey(nameof(EmployeeId))] // explicit since not called UserId
        public User User { get; set; }
    }
}

namespace OmgvaPOS.ScheduleManagement.DTOs
{
    public class EmployeeSchedulesWithAvailability
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public List<ScheduleWithAvailability>? ScheduleWithAvailabilities { get; set; }
    }
}

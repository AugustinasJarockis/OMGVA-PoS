namespace OmgvaPOS.ScheduleManagement.DTOs
{
    public class CreateEmployeeScheduleRequest
    {
        public long EmployeeId { get; set; }
        public DateOnly Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}

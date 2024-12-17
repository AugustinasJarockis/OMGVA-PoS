namespace OmgvaPOS.ScheduleManagement.DTOs
{
    public class UpdateEmployeeScheduleRequest
    {
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}

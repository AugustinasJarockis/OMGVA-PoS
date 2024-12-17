namespace OmgvaPOS.ScheduleManagement.DTOs
{
    public class ScheduleWithAvailability
    {
        public long EmployeeScheduleId { get; set; }
        public DateOnly Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public List<Timeslot>? AvailableTimeslots { get; set; }
    }
}

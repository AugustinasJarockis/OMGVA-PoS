namespace OMGVA_PoS.Data_layer.Models
{
    public class EmployeeSchedule
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public DateOnly Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsCancelled { get; set; }
    }
}

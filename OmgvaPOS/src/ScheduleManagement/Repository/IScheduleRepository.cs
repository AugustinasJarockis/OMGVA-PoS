using OmgvaPOS.ScheduleManagement.DTOs;
using OmgvaPOS.ScheduleManagement.Models;

namespace OmgvaPOS.ScheduleManagement.Repository
{
    public interface IScheduleRepository
    {
        public EmployeeSchedule Create(EmployeeSchedule employeeSchedule);
        public void Delete(long id);
        public EmployeeSchedule Update(long id, UpdateEmployeeScheduleRequest request);
        public List<EmployeeSchedule> GetSchedulesByEmployeeId(long employeeId);
        public EmployeeSchedule GetScheduleById(long id);
        public EmployeeSchedule? GetScheduleByEmployeeIdAndDate(long employeeId, DateOnly date);
    }
}

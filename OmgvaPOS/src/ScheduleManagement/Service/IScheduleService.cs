using OmgvaPOS.ScheduleManagement.DTOs;
using OmgvaPOS.ScheduleManagement.Models;

namespace OmgvaPOS.ScheduleManagement.Service
{
    public interface IScheduleService
    {
        public EmployeeSchedule CreateEmployeeSchedule(CreateEmployeeScheduleRequest createEmployeeScheduleRequest);
        public ScheduleWithAvailability GetEmployeeSchedule(long id);
        public EmployeeSchedulesWithAvailability GetEmployeeScheduleByItemAndDate(long itemId, DateOnly date);
        public EmployeeSchedulesWithAvailability GetEmployeeScheduleWithAvailability(long employeeId, DateOnly date);
        public List<EmployeeSchedule> GetAllSchedulesByEmployeeId(long employeeId);
        public long GetBusinessIdFromEmployeeSchedule(long employeeScheduleId);
        public long GetBusinessIdFromEmployee(long employeeId);
        public long GetBusinessIdFromItem(long itemId);
        public void DeleteEmployeeSchedule(long id);
        public EmployeeSchedule UpdateEmployeeSchedule(long id, UpdateEmployeeScheduleRequest updateEmployeeScheduleRequest);
    }
}

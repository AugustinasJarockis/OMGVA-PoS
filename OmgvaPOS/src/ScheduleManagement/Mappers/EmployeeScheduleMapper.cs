using OmgvaPOS.ReservationManagement.Models;
using OmgvaPOS.ScheduleManagement.DTOs;
using OmgvaPOS.ScheduleManagement.Models;

namespace OmgvaPOS.ScheduleManagement.Mappers
{
    public static class EmployeeScheduleMapper
    {
        public static Timeslot ToUnavailableTimeslot (Reservation reservation, TimeSpan duration)
        {
            return new Timeslot()
            {
                StartTime = TimeOnly.FromDateTime(reservation.TimeReserved).ToTimeSpan(),
                EndTime = TimeOnly.FromDateTime(reservation.TimeReserved).ToTimeSpan().Add(duration),
            };
        }
        public static Timeslot ToAvailableTimeslot (TimeSpan start, TimeSpan end)
        {
            return new Timeslot()
            {
                StartTime = start,
                EndTime = end
            };
        }
        public static ScheduleWithAvailability ToScheduleWithAvailability (EmployeeSchedule employeeSchedule, List<Timeslot> availableTimeslots)
        {
            return new ScheduleWithAvailability()
            {
                EmployeeScheduleId = employeeSchedule.Id,
                Date = employeeSchedule.Date,
                StartTime = employeeSchedule.StartTime,
                EndTime = employeeSchedule.EndTime,
                AvailableTimeslots = availableTimeslots
            };
        }
        public static EmployeeSchedulesWithAvailability ToEmployeeSchedulesWithAvailability (long employeeId, string employeeName, List<ScheduleWithAvailability> scheduleWithAvailabilities)
        {
            return new EmployeeSchedulesWithAvailability()
            {
                EmployeeId = employeeId,
                EmployeeName = employeeName,
                ScheduleWithAvailabilities = scheduleWithAvailabilities
            };
        }
        public static EmployeeSchedule ToCreate (CreateEmployeeScheduleRequest request)
        {
            return new EmployeeSchedule()
            {
                EmployeeId = request.EmployeeId,
                Date = request.Date,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                IsCancelled = false
            };
        }
    }
}

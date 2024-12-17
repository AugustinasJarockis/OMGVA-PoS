using OmgvaPOS.Exceptions;
using OmgvaPOS.ItemManagement.Repositories;
using OmgvaPOS.ReservationManagement.Repository;
using OmgvaPOS.ScheduleManagement.DTOs;
using OmgvaPOS.ScheduleManagement.Mappers;
using OmgvaPOS.ScheduleManagement.Models;
using OmgvaPOS.ScheduleManagement.Repository;
using OmgvaPOS.ScheduleManagement.Validators;
using OmgvaPOS.UserManagement.Repository;
using System.Data;
using System.Diagnostics.Eventing.Reader;

namespace OmgvaPOS.ScheduleManagement.Service
{
    public class ScheduleService(IScheduleRepository scheduleRepository, IReservationRepository reservationRepository, IItemRepository itemRepository, IUserRepository userRepository) : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository = scheduleRepository;
        private readonly IReservationRepository _reservationRepository = reservationRepository;
        private readonly IItemRepository _itemRepository = itemRepository;
        private readonly IUserRepository _userRepository = userRepository;
        public EmployeeSchedule CreateEmployeeSchedule(CreateEmployeeScheduleRequest createEmployeeScheduleRequest)
        {
            var employeeSchedule = _scheduleRepository.GetScheduleByEmployeeIdAndDate(createEmployeeScheduleRequest.EmployeeId, createEmployeeScheduleRequest.Date);
            if (employeeSchedule != null)
            {
                throw new ConflictException("This date is already scheduled.");
            }

            EmployeeScheduleValidator.AreStartToEndDatesValid(createEmployeeScheduleRequest.StartTime, createEmployeeScheduleRequest.EndTime);

            var createdSchedule = _scheduleRepository.Create(EmployeeScheduleMapper.ToCreate(createEmployeeScheduleRequest));
            return createdSchedule;
        }
        public void DeleteEmployeeSchedule(long id)
        {
            var employeeSchedule = _scheduleRepository.GetScheduleById(id);
            var reservations = _reservationRepository.GetByEmployeeIdAndDate(employeeSchedule.EmployeeId, employeeSchedule.Date);

            foreach ( var reservation in reservations )
            {
                _reservationRepository.Delete(reservation.Id);
            }

            _scheduleRepository.Delete(employeeSchedule.Id);
        }
        public EmployeeSchedule UpdateEmployeeSchedule(long id, UpdateEmployeeScheduleRequest updateEmployeeScheduleRequest)
        {
            var employeeSchedule = _scheduleRepository.GetScheduleById(id);
            var reservations = _reservationRepository.GetByEmployeeIdAndDate(employeeSchedule.EmployeeId, employeeSchedule.Date);
            var item = _itemRepository.GetItem(reservations.Last().ItemId);
            
            if(updateEmployeeScheduleRequest.StartTime != null)
                EmployeeScheduleValidator.IsValidStartTime(TimeOnly.FromDateTime(reservations.First().TimeReserved).ToTimeSpan(), updateEmployeeScheduleRequest.StartTime ?? TimeSpan.Zero);
            if (updateEmployeeScheduleRequest.EndTime != null && item != null)
                EmployeeScheduleValidator.IsValidEndTime(TimeOnly.FromDateTime(reservations.Last().TimeReserved).ToTimeSpan() + (item.Duration ?? TimeSpan.Zero), updateEmployeeScheduleRequest.EndTime ?? TimeSpan.Zero);

            var updatedSchedule = _scheduleRepository.Update(id, updateEmployeeScheduleRequest);

            return updatedSchedule;
        }
        public ScheduleWithAvailability GetEmployeeSchedule(long id)
        {
            var employeeSchedule = _scheduleRepository.GetScheduleById(id);
            var reservations = _reservationRepository.GetByEmployeeIdAndDate(employeeSchedule.EmployeeId, employeeSchedule.Date);
            var unavailableTimeslots = new List<Timeslot>();

            foreach(var reservation in reservations)
            {
                var item = _itemRepository.GetItem(reservation.ItemId);
                if (item != null)
                {
                    var unavailableTimeslot = EmployeeScheduleMapper.ToUnavailableTimeslot(reservation, item.Duration ?? TimeSpan.Zero);
                    unavailableTimeslots.Add(unavailableTimeslot);
                }
            }

            var availableTimeslots = CalculateAvailableTime(employeeSchedule, unavailableTimeslots);

            return EmployeeScheduleMapper.ToScheduleWithAvailability(employeeSchedule, availableTimeslots);
        }
        public List<EmployeeSchedulesWithAvailability> GetEmployeesSchedulesByItemAndDate(long itemId, DateOnly date)
        {
            var reservations = _reservationRepository.GetByItemIdAndDate(itemId, date);
            var uniqueEmployeeIds = reservations.Select(r => r.EmployeeId).Distinct().ToList();
            var employeeSchedulesByItemAndDate = new List<EmployeeSchedulesWithAvailability>();

            foreach (var employeeId in uniqueEmployeeIds)
            {
                var schedulesWithAvailability = GetEmployeeScheduleWithAvailability(employeeId, date);
                employeeSchedulesByItemAndDate.Add(schedulesWithAvailability);
            }

            return employeeSchedulesByItemAndDate;
        }
        public EmployeeSchedulesWithAvailability GetEmployeeScheduleWithAvailability(long employeeId, DateOnly date)
        {
            var employeeSchedule = _scheduleRepository.GetScheduleByEmployeeIdAndDate(employeeId, date) ?? throw new NotFoundException("Employee schedule was not found");
            var employee = _userRepository.GetUser(employeeId);
            var reservations = _reservationRepository.GetByEmployeeIdAndDate(employeeId, date);
            var unavailableTimeslots = new List<Timeslot>();

            foreach (var reservation in reservations)
            {
                var item = _itemRepository.GetItem(reservation.ItemId);
                if (item != null)
                {
                    var unavailableTimeslot = EmployeeScheduleMapper.ToUnavailableTimeslot(reservation, item.Duration ?? TimeSpan.Zero);
                    unavailableTimeslots.Add(unavailableTimeslot);
                }
            }
            var availableTimeslots = CalculateAvailableTime(employeeSchedule, unavailableTimeslots);
            var scheduleWithAvailability = new List<ScheduleWithAvailability>
            {
                EmployeeScheduleMapper.ToScheduleWithAvailability(employeeSchedule, availableTimeslots)
            };

            return EmployeeScheduleMapper.ToEmployeeSchedulesWithAvailability(employeeId, employee.Name, scheduleWithAvailability);
        }
        public List<EmployeeSchedule> GetAllSchedulesByEmployeeId(long employeeId)
        {
            var schedules = _scheduleRepository.GetSchedulesByEmployeeId(employeeId);

            return schedules;
        }
        public long GetBusinessIdFromEmployeeSchedule(long employeeScheduleId)
        {
            var schedule = _scheduleRepository.GetScheduleById(employeeScheduleId);
            var user = _userRepository.GetUserNoException(schedule.EmployeeId);
            return user.BusinessId ?? -1;
        }
        public long GetBusinessIdFromEmployee(long employeeId)
        {
            var user = _userRepository.GetUserNoException(employeeId);
            return user.BusinessId ?? -1;
        }
        public long GetBusinessIdFromItem(long itemId)
        {
            var item = _itemRepository.GetItem(itemId);
            return item.BusinessId;
        }
        private static List<Timeslot> CalculateAvailableTime(EmployeeSchedule schedule, List<Timeslot> unavailableTimeslots)
        {
            var availableTimeslots = new List<Timeslot>();
            TimeSpan currentStart = schedule.StartTime;

            foreach (var unavailableTimeslot in unavailableTimeslots.OrderBy(t => t.StartTime))
            {
                if (unavailableTimeslot.StartTime > currentStart)
                {
                    availableTimeslots.Add(EmployeeScheduleMapper.ToAvailableTimeslot(currentStart, unavailableTimeslot.StartTime));
                }

                currentStart = unavailableTimeslot.EndTime;
            }

            if (currentStart < schedule.EndTime)
            {
                availableTimeslots.Add(EmployeeScheduleMapper.ToAvailableTimeslot(currentStart, schedule.EndTime));
            }

            return availableTimeslots;
        }
    }
}

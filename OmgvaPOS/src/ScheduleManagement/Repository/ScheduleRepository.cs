using Microsoft.EntityFrameworkCore;
using OmgvaPOS.Database.Context;
using OmgvaPOS.Exceptions;
using OmgvaPOS.ScheduleManagement.DTOs;
using OmgvaPOS.ScheduleManagement.Models;

namespace OmgvaPOS.ScheduleManagement.Repository
{
    public class ScheduleRepository(OmgvaDbContext database) : IScheduleRepository
    {
        private readonly OmgvaDbContext _database = database;
        public EmployeeSchedule Create (EmployeeSchedule employeeSchedule)
        {
            var createdSchedule = _database.EmployeeSchedules.Add(employeeSchedule).Entity;
            _database.SaveChanges();
            return createdSchedule;
        }
        public void Delete (long id)
        {
            var employeeSchedule = _database.EmployeeSchedules.Find(id) ?? throw new NotFoundException("Schedule was not found");
            if (employeeSchedule != null)
            {
                employeeSchedule.IsCancelled = true;
                _database.SaveChanges();
            }
        }
        public EmployeeSchedule Update (long id, UpdateEmployeeScheduleRequest request)
        {
            var employeeSchedule = _database.EmployeeSchedules.First(e => e.Id == id);

            employeeSchedule.StartTime = request.StartTime ?? employeeSchedule.StartTime;
            employeeSchedule.EndTime = request.EndTime ?? employeeSchedule.EndTime;

            _database.SaveChanges();

            return employeeSchedule;
        }
        public List<EmployeeSchedule> GetSchedulesByEmployeeId (long employeeId)
        {
            var employeeSchedules = _database.EmployeeSchedules.Where(es => es.EmployeeId == employeeId && !es.IsCancelled);
            if (employeeSchedules == null || !employeeSchedules.Any())
            {
                throw new NotFoundException($"No schedules found for employee with ID {employeeId}.");
            }

            return [.. employeeSchedules];
        }
        public EmployeeSchedule GetScheduleById (long id)
        {
            var employeeSchedule = _database.EmployeeSchedules.First(es => es.Id == id) ?? throw new NotFoundException("Schedule not found.");
            return employeeSchedule;
        }
        public EmployeeSchedule? GetScheduleByEmployeeIdAndDate(long employeeId, DateOnly date)
        {
            var employeeSchedule = _database.EmployeeSchedules.FirstOrDefault(es => es.EmployeeId == employeeId && !es.IsCancelled && es.Date == date);
            return employeeSchedule;
        }
    }
}

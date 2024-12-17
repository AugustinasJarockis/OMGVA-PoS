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

        public EmployeeSchedule Create(EmployeeSchedule employeeSchedule)
        {
            var user = _database.Users.Find(employeeSchedule.EmployeeId);
            if (user == null)
                throw new NotFoundException("Employee not found.");

            var createdSchedule = _database.EmployeeSchedules.Add(employeeSchedule).Entity;
            _database.SaveChanges();

            _database.Entry(createdSchedule).Reference(es => es.User).Load();

            return createdSchedule;
        }

        public void Delete(long id)
        {
            var employeeSchedule = _database.EmployeeSchedules.Find(id)
                ?? throw new NotFoundException("Schedule was not found.");

            // Soft delete: Mark the schedule as cancelled
            employeeSchedule.IsCancelled = true;
            _database.SaveChanges();
        }

        public EmployeeSchedule Update(long id, UpdateEmployeeScheduleRequest request)
        {
            var employeeSchedule = _database.EmployeeSchedules
                .Include(es => es.User)
                .FirstOrDefault(e => e.Id == id)
                ?? throw new NotFoundException("Schedule not found.");

            employeeSchedule.StartTime = request.StartTime ?? employeeSchedule.StartTime;
            employeeSchedule.EndTime = request.EndTime ?? employeeSchedule.EndTime;

            _database.SaveChanges();
            return employeeSchedule;
        }

        public List<EmployeeSchedule> GetSchedulesByEmployeeId(long employeeId)
        {
            var userExists = _database.Users.Any(u => u.Id == employeeId);
            if (!userExists)
                throw new NotFoundException($"Employee with ID {employeeId} does not exist.");

            var employeeSchedules = _database.EmployeeSchedules
                .Where(es => es.EmployeeId == employeeId && !es.IsCancelled)
                .ToList();

            if (!employeeSchedules.Any())
                throw new NotFoundException($"No schedules found for employee with ID {employeeId}.");

            return employeeSchedules;
        }

        public EmployeeSchedule GetScheduleById(long id)
        {
            var employeeSchedule = _database.EmployeeSchedules
                .Include(es => es.User)
                .FirstOrDefault(es => es.Id == id)
                ?? throw new NotFoundException("Schedule not found.");

            return employeeSchedule;
        }

        public EmployeeSchedule? GetScheduleByEmployeeIdAndDate(long employeeId, DateOnly date)
        {
            var employeeSchedule = _database.EmployeeSchedules
                .FirstOrDefault(es => es.EmployeeId == employeeId && !es.IsCancelled && es.Date == date);

            return employeeSchedule;
        }
    }
}

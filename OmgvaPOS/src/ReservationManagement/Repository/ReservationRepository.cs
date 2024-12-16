using Microsoft.EntityFrameworkCore;
using OmgvaPOS.Database.Context;
using OmgvaPOS.ReservationManagement.Enums;
using OmgvaPOS.ReservationManagement.Models;

namespace OmgvaPOS.ReservationManagement.Repository;

public class ReservationRepository : IReservationRepository
{
    private readonly OmgvaDbContext _context;

    public ReservationRepository(OmgvaDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Reservation> GetAll()
    {
        return _context.Reservations
            .Include(r => r.User)
            .Include(r => r.Customer)
            .ToList();
    }

    public Reservation? GetById(long id)
    {
        return _context.Reservations
            .Include(r => r.User)
            .Include(r => r.Customer)
            .FirstOrDefault(r => r.Id == id);
    }
    public List<Reservation> GetByItemIdAndDate(long itemId, DateOnly date)
    {
        return [.. _context.Reservations.Where(r => r.ItemId == itemId && DateOnly.FromDateTime(r.TimeReserved) == date && r.Status == Enums.ReservationStatus.Open)];
    }
    public List<Reservation> GetByEmployeeIdAndDate(long id, DateOnly date)
    {
        return [.. _context.Reservations.Where(r => r.EmployeeId == id && DateOnly.FromDateTime(r.TimeReserved) == date && r.Status == Enums.ReservationStatus.Open).OrderBy(r => r.TimeReserved)];
    }

    public Reservation Create(Reservation reservation)
    {
        _context.Reservations.Add(reservation);
        _context.SaveChanges();
        return reservation;
    }

    public Reservation Update(Reservation reservation)
    {
        _context.Reservations.Update(reservation);
        _context.SaveChanges();
        return reservation;
    }

    public void Delete(long id)
    {
        var reservation = _context.Reservations.Find(id);
        if (reservation != null)
        {
            reservation.Status = ReservationStatus.Cancelled;
            _context.SaveChanges();
        }
    }
}
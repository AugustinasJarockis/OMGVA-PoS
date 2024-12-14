using Microsoft.EntityFrameworkCore;
using OmgvaPOS.Database.Context;
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
            _context.Reservations.Remove(reservation);
            _context.SaveChanges();
        }
    }
}
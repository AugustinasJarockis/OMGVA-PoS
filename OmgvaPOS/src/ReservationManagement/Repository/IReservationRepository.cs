using OmgvaPOS.ReservationManagement.Models;

namespace OmgvaPOS.ReservationManagement.Repository;

public interface IReservationRepository
{
    IEnumerable<Reservation> GetAll();
    Reservation? GetById(long id);
    Reservation Create(Reservation reservation);
    Reservation Update(Reservation reservation);
    void Delete(long id);
}

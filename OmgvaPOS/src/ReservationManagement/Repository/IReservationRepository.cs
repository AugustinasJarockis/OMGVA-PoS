using OmgvaPOS.ReservationManagement.Models;

namespace OmgvaPOS.ReservationManagement.Repository;

public interface IReservationRepository
{
    IEnumerable<Reservation> GetAll();
    Reservation? GetById(long id);
    IEnumerable<Reservation> GetByEmployeeId(long employeeId);
    public List<Reservation> GetByItemIdAndDate(long itemId, DateOnly date);
    public List<Reservation> GetByEmployeeIdAndDate(long id, DateOnly date);
    Reservation Create(Reservation reservation);
    Reservation Update(Reservation reservation);
    void Delete(long id);
}

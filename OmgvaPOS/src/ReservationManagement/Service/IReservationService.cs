using OmgvaPOS.ReservationManagement.DTOs;
using OmgvaPOS.ReservationManagement.Enums;

namespace OmgvaPOS.ReservationManagement.Service;

public interface IReservationService
{
    IEnumerable<ReservationDto> GetAll();
    ReservationDto? GetById(long id);
    ReservationDto Create(CreateReservationDto createDto);
    ReservationDto Update(long id, UpdateReservationDto updateDto);
    void Delete(long id);
}
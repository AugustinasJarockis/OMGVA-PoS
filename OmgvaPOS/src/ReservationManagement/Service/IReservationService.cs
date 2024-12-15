using OmgvaPOS.ReservationManagement.DTOs;
using OmgvaPOS.ReservationManagement.Enums;

namespace OmgvaPOS.ReservationManagement.Service;

public interface IReservationService
{
    IEnumerable<ReservationDto> GetAll();
    ReservationDto? GetById(long id);
    ReservationDto Create(CreateReservationRequest createRequest);
    ReservationDto Update(long id, UpdateReservationRequest updateRequest, long businessId);
    void Delete(long id);
}
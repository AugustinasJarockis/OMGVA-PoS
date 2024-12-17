using OmgvaPOS.ReservationManagement.DTOs;

namespace OmgvaPOS.ReservationManagement.Service;

public interface IReservationService
{
    IEnumerable<ReservationDto> GetAll();
    ReservationDto? GetById(long id);
    IEnumerable<ReservationDto> GetEmployeeReservations(long employeeId);
    ReservationDto Create(CreateReservationRequest createRequest);
    ReservationDto Update(long id, UpdateReservationRequest updateRequest, long businessId);
    void Delete(long id);
}
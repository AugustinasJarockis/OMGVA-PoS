using OmgvaPOS.Exceptions;
using OmgvaPOS.ReservationManagement.DTOs;
using OmgvaPOS.ReservationManagement.Mappers;
using OmgvaPOS.ReservationManagement.Repository;
using OmgvaPOS.ReservationManagement.Validators;
using static OmgvaPOS.Exceptions.ExceptionErrors;

namespace OmgvaPOS.ReservationManagement.Service
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _repository;

        public ReservationService(IReservationRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<ReservationDto> GetAll()
        {
            var reservations = _repository.GetAll();
            return reservations.ToDtoList();
        }

        public ReservationDto? GetById(long id)
        {
            var reservation = _repository.GetById(id);
            return reservation?.ToDto();
        }

        // TODO: make sure employee is available at that time (check schedule)
        public ReservationDto Create(CreateReservationRequest createRequest)
        {
            ReservationValidator.ValidateCreateReservationRequest(createRequest);
            var reservation = createRequest.ToModel();
            
            var createdReservation = _repository.Create(reservation);
            return createdReservation.ToDto();
        }

        // TODO: think about historic data for reservation UPDATE
        // for example cannot update if reservation is complete
        public ReservationDto Update(long id, UpdateReservationRequest updateRequest)
        {
            var existingReservation = _repository.GetById(id);
            if (existingReservation == null)
                throw new NotFoundException(GetReservationNotFoundMessage(id));

            existingReservation.UpdateEntity(updateRequest);
            
            var updatedReservation = _repository.Update(existingReservation);
            return updatedReservation.ToDto();
        }
    
        // TODO: think about historic data for reservation DELETE
        public void Delete(long id)
        {
            var reservation = _repository.GetById(id);
            if (reservation == null)
                throw new NotFoundException(GetReservationNotFoundMessage(id));

            _repository.Delete(id);
        }

    }
}
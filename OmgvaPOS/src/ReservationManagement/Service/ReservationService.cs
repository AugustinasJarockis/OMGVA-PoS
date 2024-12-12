using OmgvaPOS.Exceptions;
using OmgvaPOS.ReservationManagement.DTOs;
using OmgvaPOS.ReservationManagement.Mappers;
using OmgvaPOS.ReservationManagement.Repository;
using static OmgvaPOS.Exceptions.ExceptionMessages;

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
            return ReservationMapper.ToDtoList(reservations);
        }

        public ReservationDto? GetById(long id)
        {
            var reservation = _repository.GetById(id);
            return reservation == null ? null : ReservationMapper.ToDto(reservation);
        }

        public ReservationDto Create(CreateReservationDto createDto)
        {
            var reservation = ReservationMapper.ToEntity(createDto);
            
            var createdReservation = _repository.Create(reservation);
            return ReservationMapper.ToDto(createdReservation);
        }

        public ReservationDto Update(long id, UpdateReservationDto updateDto)
        {
            var existingReservation = _repository.GetById(id);
            if (existingReservation == null)
                throw new NotFoundException(ReservationNotFoundMessage(id));

            ReservationMapper.UpdateEntity(existingReservation, updateDto);
            
            var updatedReservation = _repository.Update(existingReservation);
            return ReservationMapper.ToDto(updatedReservation);
        }

        public void Delete(long id)
        {
            var reservation = _repository.GetById(id);
            if (reservation == null)
                throw new NotFoundException(ReservationNotFoundMessage(id));

            _repository.Delete(id);
        }

    }
}
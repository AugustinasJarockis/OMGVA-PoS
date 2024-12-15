using OmgvaPOS.CustomerManagement.Service;
using OmgvaPOS.Exceptions;
using OmgvaPOS.ReservationManagement.DTOs;
using OmgvaPOS.ReservationManagement.Mappers;
using OmgvaPOS.ReservationManagement.Models;
using OmgvaPOS.ReservationManagement.Repository;
using OmgvaPOS.ReservationManagement.Validators;
using OmgvaPOS.UserManagement.Service;
using static OmgvaPOS.Exceptions.ExceptionErrors;

namespace OmgvaPOS.ReservationManagement.Service
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _repository;
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;

        public ReservationService(IReservationRepository repository, ICustomerService customerService, IUserService userService)
        {
            _repository = repository;
            _customerService = customerService;
            _userService = userService;
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

            var customer = _customerService.GetById(createRequest.CustomerId);
            if (customer == null)
                throw new NotFoundException($"Cannot find customer with ID {createRequest.CustomerId}");
            
            var employee = _userService.GetUser(createRequest.EmployeeId);
            if (employee == null)
                throw new NotFoundException($"Cannot find employee with ID {createRequest.EmployeeId}");
            
            var createdReservation = _repository.Create(reservation);
            return createdReservation.ToDto();
        }

        // TODO: think about historic data for reservation UPDATE
        // for example cannot update if reservation is complete
        public ReservationDto Update(long id, UpdateReservationRequest updateRequest, long businessId)
        {
            var existingReservation = GetReservationOrThrow(id);

            _userService.ValidateUserBelongsToBusiness(updateRequest.EmployeeId, businessId);
            // TODO: add check that customer is present
            
            existingReservation.UpdateEntity(updateRequest);
            
            var updatedReservation = _repository.Update(existingReservation);
            return updatedReservation.ToDto();
        }
    
        public void Delete(long id)
        {
            var reservation = GetReservationOrThrow(id);

            _repository.Delete(reservation.Id);
        }

        private Reservation GetReservationOrThrow(long reservationId)
        {
            var reservation = _repository.GetById(reservationId);
            if (reservation == null)
                throw new NotFoundException(GetReservationNotFoundMessage(reservationId));

            return reservation;
        }

    }
}
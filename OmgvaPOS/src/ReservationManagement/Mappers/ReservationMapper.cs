using OmgvaPOS.ReservationManagement.DTOs;
using OmgvaPOS.ReservationManagement.Enums;
using OmgvaPOS.ReservationManagement.Models;

namespace OmgvaPOS.ReservationManagement.Mappers
{
    public static class ReservationMapper
    {
        public static ReservationDto ToDto(this Reservation reservation)
        {
            return new ReservationDto
            {
                Id = reservation.Id,
                TimeCreated = reservation.TimeCreated,
                TimeReserved = reservation.TimeReserved,
                Status = reservation.Status,
                EmployeeId = reservation.EmployeeId,
                CustomerId = reservation.CustomerId,
                EmployeeName = reservation.User.Name,
                CustomerName = reservation.Customer.Name
            };
        }

        public static IEnumerable<ReservationDto> ToDtoList(this IEnumerable<Reservation> reservations)
        {
            return reservations.Select(ToDto).ToList();
        }

        public static Reservation ToModel(this CreateReservationRequest request)
        {
            return new Reservation
            {
                TimeCreated = DateTime.UtcNow,
                Status = ReservationStatus.Open,
                
                TimeReserved = request.TimeReserved,
                EmployeeId = request.EmployeeId,
                CustomerId = request.CustomerId
            };
        }

        public static void UpdateEntity(this Reservation entity, UpdateReservationRequest updateRequest)
        {
            entity.TimeReserved = updateRequest.TimeReserved ?? entity.TimeReserved;
            entity.Status = updateRequest.Status ?? entity.Status;
            entity.EmployeeId = updateRequest.EmployeeId ?? entity.EmployeeId;
            entity.CustomerId = updateRequest.CustomerId ?? entity.CustomerId;
        }
    }
}
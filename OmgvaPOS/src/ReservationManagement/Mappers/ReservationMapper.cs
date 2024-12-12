using OmgvaPOS.ReservationManagement.DTOs;
using OmgvaPOS.ReservationManagement.Enums;
using OmgvaPOS.ReservationManagement.Models;

namespace OmgvaPOS.ReservationManagement.Mappers
{
    public static class ReservationMapper
    {
        public static ReservationDto ToDto(Reservation reservation)
        {
            return new ReservationDto
            {
                Id = reservation.Id,
                TimeCreated = reservation.TimeCreated,
                TimeReserved = reservation.TimeReserved,
                Status = reservation.Status,
                EmployeeId = reservation.EmployeeId,
                CustomerId = reservation.CustomerId,
                EmployeeName = reservation.User != null 
                    ? $"{reservation.User.Name} {reservation.User.Name}"
                    : string.Empty,
                CustomerName = reservation.Customer != null 
                    ? $"{reservation.Customer.Name} {reservation.Customer.Name}"
                    : string.Empty
            };
        }

        public static IEnumerable<ReservationDto> ToDtoList(IEnumerable<Reservation> reservations)
        {
            if (reservations == null)
                return new List<ReservationDto>();

            return reservations.Select(ToDto).ToList();
        }

        public static Reservation ToEntity(CreateReservationDto dto)
        {
            return new Reservation
            {
                TimeCreated = DateTime.UtcNow,
                Status = ReservationStatus.Open,
                
                TimeReserved = dto.TimeReserved,
                EmployeeId = dto.EmployeeId,
                CustomerId = dto.CustomerId
            };
        }

        public static void UpdateEntity(Reservation entity, UpdateReservationDto dto)
        {
            entity.TimeReserved = dto.TimeReserved;
            entity.Status = dto.Status;
            entity.EmployeeId = dto.EmployeeId;
        }
    }
}
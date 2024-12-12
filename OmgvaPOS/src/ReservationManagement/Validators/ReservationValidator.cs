using OmgvaPOS.Exceptions;
using OmgvaPOS.ReservationManagement.DTOs;

namespace OmgvaPOS.ReservationManagement.Validators;

public class ReservationValidator
{
    public static void ValidateCreateReservationRequest(CreateReservationRequest createReservationRequest)
    {
        if (createReservationRequest.TimeReserved <= DateTime.Now)
            throw new ValidationException("Reservation time must be in the future");
    }
}
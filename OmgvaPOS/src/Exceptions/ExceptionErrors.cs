namespace OmgvaPOS.Exceptions;

public class ExceptionErrors
{
    public static string GetReservationNotFoundMessage(long id) => $"Reservation with ID {id} not found.";
    public static ErrorResponse GetReservationNotFoundErrorResponse(long id) => new(GetReservationNotFoundMessage(id));
    
    public static string GetForbiddenReservationErrorMessage(long id) => $"Cannot access reservation with ID {id}.";
}
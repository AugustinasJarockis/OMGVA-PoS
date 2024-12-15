namespace OmgvaPOS.Exceptions;

public class ForbiddenException(string message = "Forbidden Resource") : Exception(message);
namespace OmgvaPOS.Exceptions;

public class BadRequestException(string message = "Bad Request") : Exception(message);
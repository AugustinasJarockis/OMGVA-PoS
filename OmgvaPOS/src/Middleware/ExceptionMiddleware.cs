using OmgvaPOS.Exceptions;

namespace OmgvaPOS.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = new ErrorResponse();
        
        switch (exception)
        {
            case BadRequestException:
                _logger.LogWarning(exception, exception.Message);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = exception.Message;
                break;
                
            case NotFoundException:
                _logger.LogWarning(exception, exception.Message);
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = exception.Message;
                break;
            
            case ValidationException:
                _logger.LogWarning(exception, exception.Message);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = exception.Message;
                break;

            case ConflictException:
                _logger.LogWarning(exception, exception.Message);
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                response.Message = exception.Message;
                break;
            
            case ApplicationException:
                _logger.LogWarning(exception, exception.Message);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Message = exception.Message;
                break;
            
            case ForbiddenException:
                _logger.LogWarning(exception, exception.Message);
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                response.Message = exception.Message;
                break;
            
            default:
                _logger.LogError(exception, "An unexpected error occurred.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Message = "An internal server error occurred.";
                break;
        }
        
        await context.Response.WriteAsJsonAsync(response);
    }
    
}
using Microsoft.AspNetCore.Diagnostics;

namespace GymManagement.Presentation.ExceptionHandlers;

public class CustomExceptionHandler : IExceptionHandler
{
    private readonly ILogger<CustomExceptionHandler> _logger;

    public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
     
        _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);


        return false;
    }
}
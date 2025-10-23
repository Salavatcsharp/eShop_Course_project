using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions.Handler;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError("Error: {0}, Datetime {1}", exception.Message, DateTime.UtcNow);
        
        (string Detail, string Title, int StatusCode) problemDetails = exception switch
        {
            InternalServerException => 
                (
                    exception.Message,
                    exception.GetType().Name, 
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError),
            ValidationException =>
                (
                    exception.Message,
                    exception.GetType().Name, 
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest),
            BadRequestException =>
                (
                    exception.Message,
                    exception.GetType().Name, 
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest),
            NotFoundException => 
                (
                    exception.Message,
                    exception.GetType().Name, 
                    httpContext.Response.StatusCode = StatusCodes.Status404NotFound),
            _ => 
                (
                    exception.Message,
                    exception.GetType().Name, 
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError)
        };
        
        var problemDetailsResponse = new ProblemDetails
        {
            Title = problemDetails.Title,
            Detail = problemDetails.Detail,
            Status = problemDetails.StatusCode
        };
        
        problemDetailsResponse.Extensions.Add("traceId", httpContext.TraceIdentifier);

        if (exception is ValidationException validationException)
        {
            problemDetailsResponse.Extensions.Add("ValidationErrors", validationException.Errors);
        }
        
        await httpContext.Response.WriteAsJsonAsync(problemDetailsResponse, cancellationToken);
        return true;
    }
}
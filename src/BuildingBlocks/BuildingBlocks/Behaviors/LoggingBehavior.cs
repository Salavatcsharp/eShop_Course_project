using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse> (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("[START] Handling {RequestName} - Response Type: {ResponseType} - DateTime: {DateTime}",
            typeof(TRequest).Name, typeof(TResponse).Name, DateTime.UtcNow);

        var timer = new Stopwatch();
        timer.Start();
        
        var response = await next();
        
        timer.Stop();
        var elapsed = timer.Elapsed;
        if (elapsed.TotalMilliseconds > 3000)
        {
            logger.LogWarning("[SLOW] Handled {RequestName} in {ElapsedMilliseconds} ms", typeof(TRequest).Name, elapsed.TotalMilliseconds);
            logger.LogWarning("Request Details: {@Request}", request);
        }
        else
        {
            logger.LogInformation("[END] Handled {RequestName} in {ElapsedMilliseconds} ms", typeof(TRequest).Name, elapsed.TotalMilliseconds);
        }
        
        return response;
    }
}
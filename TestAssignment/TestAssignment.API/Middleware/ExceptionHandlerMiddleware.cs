using System.Net;
using System.Text.Json;

namespace TestAssignment.API.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            await HandleExceptionAsync(context, ex).ConfigureAwait(false);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";
        var statusCode = (int)HttpStatusCode.InternalServerError;

        context.Response.StatusCode = statusCode;

        var result = JsonSerializer.Serialize(new
        {
            StatusCode = statusCode,
            ErrorMessage = exception.Message,
#if DEBUG
            Exception = exception.ToString(),
#endif
        });
        return context.Response.WriteAsync(result);
    }
}
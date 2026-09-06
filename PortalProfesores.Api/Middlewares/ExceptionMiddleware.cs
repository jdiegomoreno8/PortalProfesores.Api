using System.Net;
using System.Text.Json;

namespace PortalProfesores.Api.Middleware;

/// <summary>
/// Middleware encargado de capturar las excepciones no controladas
/// y devolver una respuesta uniforme al cliente.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
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

    /// <summary>
    /// Construye la respuesta de error.
    /// </summary>
    private static Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";

        context.Response.StatusCode = exception switch
        {
            ArgumentException => (int)HttpStatusCode.BadRequest,
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var response = new
        {
            Success = false,
            Message = exception.Message
        };

        return context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}
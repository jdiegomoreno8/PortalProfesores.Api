using System.Net;
using System.Text.Json;
using PortalProfesores.Domain.Exceptions;

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
            ValidationException => (int)HttpStatusCode.BadRequest,

            UnauthorizedException => (int)HttpStatusCode.Unauthorized,

            NotFoundException => (int)HttpStatusCode.NotFound,

            ConflictException => (int)HttpStatusCode.Conflict,

            ArgumentException => (int)HttpStatusCode.BadRequest,

            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,

            _ => (int)HttpStatusCode.InternalServerError
        };

        var response = new
        {
            success = false,
            statusCode = context.Response.StatusCode,
            message = exception.Message
        };

        return context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}
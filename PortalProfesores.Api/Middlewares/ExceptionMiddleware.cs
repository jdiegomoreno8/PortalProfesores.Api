
using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PortalProfesores.Domain.Exceptions;

namespace PortalProfesores.Api.Middleware;

/// <summary>
/// Captura excepciones no controladas y devuelve
/// respuestas de error con un formato uniforme.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogError(
                    ex,
                    "Ocurrió un error después de iniciar la respuesta HTTP.");

                throw;
            }

            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var statusCode = exception switch
        {
            ValidationException =>
                (int)HttpStatusCode.BadRequest,

            UnauthorizedException =>
                (int)HttpStatusCode.Unauthorized,

            NotFoundException =>
                (int)HttpStatusCode.NotFound,

            ConflictException =>
                (int)HttpStatusCode.Conflict,

            ArgumentException =>
                (int)HttpStatusCode.BadRequest,

            UnauthorizedAccessException =>
                (int)HttpStatusCode.Forbidden,

            DbUpdateException =>
                (int)HttpStatusCode.InternalServerError,

            _ =>
                (int)HttpStatusCode.InternalServerError
        };

        var message = statusCode switch
        {
            400 or 401 or 404 or 409 => exception.Message,
            403 => "No tienes permiso para realizar esta operación.",
            _ => "Ocurrió un error interno. Inténtalo nuevamente más tarde."
        };

        if (statusCode >= 500)
        {
            _logger.LogError(
                exception,
                "Error interno en {Method} {Path}.",
                context.Request.Method,
                context.Request.Path);
        }
        else
        {
            _logger.LogWarning(
                "Solicitud rechazada: {StatusCode} en {Method} {Path}. Motivo: {Message}",
                statusCode,
                context.Request.Method,
                context.Request.Path,
                exception.Message);
        }

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json; charset=utf-8";

        var response = new
        {
            success = false,
            statusCode,
            message
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}

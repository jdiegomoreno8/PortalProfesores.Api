
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalProfesores.Domain.DTOs.Profesores;
using PortalProfesores.Domain.Interfaces.Services;

namespace PortalProfesores.Api.Controllers;

/// <summary>
/// Expone los endpoints relacionados con la gestión
/// del perfil del profesor autenticado.
/// </summary>
[ApiController]
[Authorize(Roles = "Profesor")]
[Route("api/[controller]")]
public class ProfesoresController : ControllerBase
{
    private readonly IProfesorService _profesorService;

    public ProfesoresController(
        IProfesorService profesorService)
    {
        _profesorService = profesorService;
    }

    /// <summary>
    /// Obtiene la información del profesor autenticado.
    /// </summary>
    [HttpGet("perfil")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObtenerPerfil()
    {
        if (!long.TryParse(
                User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var idUsuario))
        {
            return Unauthorized(new
            {
                success = false,
                statusCode = StatusCodes.Status401Unauthorized,
                message = "No fue posible identificar al usuario autenticado."
            });
        }

        var perfil =
            await _profesorService.ObtenerPerfilAsync(idUsuario);

        if (perfil is null)
        {
            return NotFound(new
            {
                success = false,
                statusCode = StatusCodes.Status404NotFound,
                message = "No se encontró el perfil del profesor."
            });
        }

        return Ok(new
        {
            success = true,
            statusCode = StatusCodes.Status200OK,
            message = "Perfil consultado correctamente.",
            data = perfil
        });
    }

    /// <summary>
    /// Actualiza la información del profesor autenticado.
    /// </summary>
    [HttpPut("perfil")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ActualizarPerfil(
        [FromBody] ActualizarProfesorRequestDto request)
    {
        if (!long.TryParse(
                User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var idUsuario))
        {
            return Unauthorized(new
            {
                success = false,
                statusCode = StatusCodes.Status401Unauthorized,
                message = "No fue posible identificar al usuario autenticado."
            });
        }

        await _profesorService.ActualizarPerfilAsync(
            idUsuario,
            request);

        return Ok(new
        {
            success = true,
            statusCode = StatusCodes.Status200OK,
            message = "Perfil actualizado correctamente."
        });
    }

    /// <summary>
    /// Cambia la fotografía del profesor autenticado.
    /// </summary>
    [HttpPut("perfil/foto")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CambiarFotoPerfil(
    IFormFile archivo)
    {
        if (archivo == null || archivo.Length == 0)
        {
            return BadRequest(new
            {
                success = false,
                statusCode = StatusCodes.Status400BadRequest,
                message = "Debe seleccionar una fotografía."
            });
        }

        if (archivo.Length > 5 * 1024 * 1024)
        {
            return BadRequest(new
            {
                success = false,
                statusCode = StatusCodes.Status400BadRequest,
                message = "La fotografía no puede superar los 5 MB."
            });
        }


        var extension = Path.GetExtension(archivo.FileName);

        if (!string.Equals(
                extension,
                ".png",
                StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(
                archivo.ContentType,
                "image/png",
                StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(new
            {
                success = false,
                statusCode = StatusCodes.Status400BadRequest,
                message = "Formato de imagen no válido. Solo se permiten archivos PNG; no se permiten JPG ni JPEG."
            });
        }


        if (!long.TryParse(
                User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var idUsuario))
        {
            return Unauthorized(new
            {
                success = false,
                statusCode = StatusCodes.Status401Unauthorized,
                message = "No fue posible identificar al usuario autenticado."
            });
        }

        await using var stream = archivo.OpenReadStream();

        await _profesorService.CambiarFotoPerfilAsync(
            idUsuario,
            stream);

        return Ok(new
        {
            success = true,
            statusCode = StatusCodes.Status200OK,
            message = "Fotografía de perfil actualizada correctamente."
        });
    }
}

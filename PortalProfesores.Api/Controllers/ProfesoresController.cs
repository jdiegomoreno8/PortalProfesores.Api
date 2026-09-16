using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalProfesores.Domain.DTOs.Profesores;
using PortalProfesores.Domain.Interfaces.Services;

namespace PortalProfesores.Api.Controllers;

/// <summary>
/// Expone los endpoints relacionados con la administración
/// del perfil del profesor.
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
    public async Task<IActionResult> ObtenerPerfil()
    {
        var idUsuarioClaim =
            User.FindFirst(ClaimTypes.NameIdentifier);

        if (idUsuarioClaim == null)
        {
            return Unauthorized();
        }

        var perfil = await _profesorService.ObtenerPerfilAsync(
            long.Parse(idUsuarioClaim.Value));

        return Ok(perfil);
    }

    /// <summary>
    /// Actualiza la información del profesor autenticado.
    /// </summary>
    [HttpPut("perfil")]
    public async Task<IActionResult> ActualizarPerfil(
        ActualizarProfesorRequestDto request)
    {
        var idUsuarioClaim =
            User.FindFirst(ClaimTypes.NameIdentifier);

        if (idUsuarioClaim == null)
        {
            return Unauthorized();
        }

        await _profesorService.ActualizarPerfilAsync(
            long.Parse(idUsuarioClaim.Value),
            request);

        return Ok(new ActualizarProfesorResponseDto
        {
            Mensaje = "Perfil actualizado correctamente."
        });
    }
}
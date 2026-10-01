using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalProfesores.Domain.DTOs.Profesores;
using PortalProfesores.Domain.Interfaces.Services;

namespace PortalProfesores.Api.Controllers;

/// <summary>
/// Expone las operaciones administrativas relacionadas
/// con la gestión de profesores.
/// </summary>
[ApiController]
[Authorize(Roles = "Administrador")]
[Route("api/[controller]")]
public class AdministradoresController : ControllerBase
{
    private readonly IAdministradorService _administradorService;

    public AdministradoresController(
        IAdministradorService administradorService)
    {
        _administradorService = administradorService;
    }

    /// <summary>
    /// Crea un nuevo profesor.
    /// Esta operación solo puede ser ejecutada por un Administrador.
    /// </summary>
    [HttpPost("profesores")]
    public async Task<IActionResult> CrearProfesor(
        CrearProfesorRequestDto request)
    {
        await _administradorService.CrearProfesorAsync(request);

        return Ok(new
        {
            Mensaje = "Profesor creado correctamente."
        });
    }

    /// <summary>
    /// Obtiene el listado de profesores registrados.
    /// </summary>
    [HttpGet("profesores")]
    public async Task<IActionResult> ObtenerProfesores()
    {
        var profesores =
            await _administradorService.ObtenerProfesoresAsync();

        return Ok(profesores);
    }
    /// <summary>
    /// Obtiene el detalle de profesores registrados.
    /// </summary>

    [HttpGet("profesores/{idProfesor:long}")]
    public async Task<IActionResult> ObtenerDetalleProfesor(long idProfesor)
    {
        var profesor =
            await _administradorService.ObtenerDetalleProfesorAsync(idProfesor);

        if (profesor is null)
        {
            return NotFound(new
            {
                Mensaje = "El profesor no fue encontrado."
            });
        }

        return Ok(profesor);
    }

    [HttpPut("profesores/{idProfesor:long}")]
    public async Task<IActionResult> EditarProfesor(
    long idProfesor,
    EditarProfesorRequestDto request)
    {
        await _administradorService.EditarProfesorAsync(
            idProfesor,
            request);

        return Ok(new
        {
            Mensaje = "Profesor actualizado correctamente."
        });
    }

    /// <summary>
    /// Cambia el estado de un profesor.
    /// Esta operación solo puede ser ejecutada por un Administrador.
    /// </summary>
    [HttpPut("profesores/{idProfesor:long}/estado")]
    public async Task<IActionResult> CambiarEstadoProfesor(
        long idProfesor,
        CambiarEstadoProfesorRequestDto request)
    {
        await _administradorService.CambiarEstadoProfesorAsync(
            idProfesor,
            request);

        return Ok(new
        {
            Mensaje = "Estado del profesor actualizado correctamente."
        });
    }
}
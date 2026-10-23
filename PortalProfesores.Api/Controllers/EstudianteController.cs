
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalProfesores.Domain.DTOs.Estudiantes;
using PortalProfesores.Domain.Interfaces.Services;

namespace PortalProfesores.Api.Controllers;

/// <summary>
/// Expone las operaciones administrativas relacionadas
/// con la consulta y actualización de estudiantes.
/// </summary>
[ApiController]
[Authorize(Roles = "Administrador")]
[Route("api/[controller]")]
public class EstudianteController : ControllerBase
{
    private readonly IEstudianteService _estudianteService;

    public EstudianteController(
        IEstudianteService estudianteService)
    {
        _estudianteService = estudianteService;
    }

    /// <summary>
    /// Obtiene el listado de estudiantes registrados.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerEstudiantes()
    {
        var estudiantes =
            await _estudianteService.ObtenerEstudiantesAsync();

        return Ok(new
        {
            success = true,
            statusCode = StatusCodes.Status200OK,
            message = "Estudiantes consultados correctamente.",
            data = estudiantes
        });
    }

    /// <summary>
    /// Obtiene el detalle de un estudiante.
    /// </summary>
    [HttpGet("{idEstudiante:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerDetalleEstudiante(
        long idEstudiante)
    {
        if (idEstudiante <= 0)
        {
            return BadRequest(new
            {
                success = false,
                statusCode = StatusCodes.Status400BadRequest,
                message = "El identificador del estudiante no es válido."
            });
        }

        var estudiante =
            await _estudianteService
                .ObtenerDetalleEstudianteAsync(idEstudiante);

        if (estudiante is null)
        {
            return NotFound(new
            {
                success = false,
                statusCode = StatusCodes.Status404NotFound,
                message = "El estudiante no fue encontrado."
            });
        }

        return Ok(new
        {
            success = true,
            statusCode = StatusCodes.Status200OK,
            message = "Estudiante consultado correctamente.",
            data = estudiante
        });
    }

    /// <summary>
    /// Actualiza la información personal y académica
    /// de un estudiante.
    /// </summary>
    [HttpPut("{idEstudiante:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditarEstudiante(
        long idEstudiante,
        [FromBody] EditarEstudianteRequest request)
    {
        if (idEstudiante <= 0)
        {
            return BadRequest(new
            {
                success = false,
                statusCode = StatusCodes.Status400BadRequest,
                message = "El identificador del estudiante no es válido."
            });
        }

        var actualizado =
            await _estudianteService.EditarEstudianteAsync(
                idEstudiante,
                request);

        if (!actualizado)
        {
            return NotFound(new
            {
                success = false,
                statusCode = StatusCodes.Status404NotFound,
                message = "El estudiante no fue encontrado."
            });
        }

        return Ok(new
        {
            success = true,
            statusCode = StatusCodes.Status200OK,
            message = "Estudiante actualizado correctamente."
        });
    }
}

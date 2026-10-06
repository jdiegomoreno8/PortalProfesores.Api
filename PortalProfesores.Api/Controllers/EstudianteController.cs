using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalProfesores.Domain.DTOs.Estudiantes;
using PortalProfesores.Domain.Interfaces.Services;

namespace PortalProfesores.Api.Controllers;

/// <summary>
/// Expone las operaciones relacionadas con la consulta
/// de estudiantes.
///
/// Este controlador recibe las solicitudes HTTP y delega
/// la lógica de negocio al EstudianteService.
/// </summary>
[ApiController]
[Authorize(Roles = "Administrador")]
[Route("api/[controller]")]
public class EstudianteController : ControllerBase
{
    private readonly IEstudianteService _estudianteService;

    /// <summary>
    /// Inicializa el controlador de estudiantes.
    /// </summary>
    /// <param name="estudianteService">
    /// Servicio encargado de la lógica de negocio
    /// relacionada con estudiantes.
    /// </param>
    public EstudianteController(
        IEstudianteService estudianteService)
    {
        _estudianteService = estudianteService;
    }

    /// <summary>
    /// Obtiene el listado de estudiantes
    /// para la gestión administrativa.
    ///
    /// El endpoint está protegido y únicamente puede
    /// ser consultado por un usuario con rol Administrador.
    /// </summary>
    /// <returns>
    /// Listado de estudiantes registrados.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> ObtenerEstudiantes()
    {
        var estudiantes =
            await _estudianteService
                .ObtenerEstudiantesAsync();

        return Ok(estudiantes);
    }

    /// <summary>
    /// Obtiene el detalle completo de un estudiante.
    ///
    /// El endpoint está protegido y únicamente puede
    /// ser consultado por un usuario con rol Administrador.
    /// </summary>
    /// <param name="idEstudiante">
    /// Identificador único del estudiante.
    /// </param>
    /// <returns>
    /// Información detallada del estudiante si existe.
    /// </returns>
    [HttpGet("{idEstudiante:long}")]
    public async Task<IActionResult> ObtenerDetalleEstudiante(
        long idEstudiante)
    {
        // Validación básica del identificador.
        // Evita realizar una consulta innecesaria a la BD
        // cuando el ID recibido no es válido.
        if (idEstudiante <= 0)
        {
            return BadRequest(new
            {
                Mensaje =
                    "El identificador del estudiante no es válido."
            });
        }

        // El controlador delega la consulta al servicio.
        // La lógica de negocio permanece fuera del Controller.
        var estudiante =
            await _estudianteService
                .ObtenerDetalleEstudianteAsync(idEstudiante);

        // Si el servicio no encuentra el estudiante,
        // devolvemos HTTP 404 Not Found.
        if (estudiante is null)
        {
            return NotFound(new
            {
                Mensaje =
                    "El estudiante no fue encontrado."
            });
        }

        // Si existe, devolvemos la información
        // mediante HTTP 200 OK.
        return Ok(estudiante);
    }

    /// <summary>
    /// Actualiza la información personal y académica
    /// de un estudiante.
    ///
    /// El endpoint está protegido y únicamente puede
    /// ser utilizado por un usuario con rol Administrador.
    /// </summary>
    /// <param name="idEstudiante">
    /// Identificador único del estudiante.
    /// </param>
    /// <param name="request">
    /// Información actualizada del estudiante.
    /// </param>
    /// <returns>
    /// HTTP 204 si la actualización fue exitosa.
    /// HTTP 400 si el identificador no es válido.
    /// HTTP 404 si el estudiante no existe.
    /// </returns>
    [HttpPut("{idEstudiante:long}")]
    public async Task<IActionResult> EditarEstudiante(
        long idEstudiante,
        [FromBody] EditarEstudianteRequest request)
    {
        // -------------------------------------------------
        // VALIDACIÓN DEL IDENTIFICADOR
        // -------------------------------------------------

        if (idEstudiante <= 0)
        {
            return BadRequest(new
            {
                Mensaje =
                    "El identificador del estudiante no es válido."
            });
        }

        // -------------------------------------------------
        // ACTUALIZAR ESTUDIANTE
        // -------------------------------------------------

        var actualizado =
            await _estudianteService
                .EditarEstudianteAsync(
                    idEstudiante,
                    request);

        // -------------------------------------------------
        // ESTUDIANTE NO ENCONTRADO
        // -------------------------------------------------

        if (!actualizado)
        {
            return NotFound(new
            {
                Mensaje =
                    "El estudiante no fue encontrado."
            });
        }

        // -------------------------------------------------
        // ACTUALIZACIÓN EXITOSA
        // -------------------------------------------------

        return NoContent();
    }
}
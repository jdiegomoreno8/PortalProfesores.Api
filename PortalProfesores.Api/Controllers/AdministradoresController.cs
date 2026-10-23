using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalProfesores.Domain.DTOs.Estudiantes;
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
        long idProfesor =
            await _administradorService.CrearProfesorAsync(request);

        return Ok(new
        {
            IdProfesor = idProfesor,
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

    /// <summary>
    /// Cambia la fotografía de perfil de un profesor.
    /// Esta operación solo puede ser ejecutada por un Administrador.
    /// </summary>
    [HttpPut("profesores/{idProfesor:long}/foto")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CambiarFotoPerfilProfesor(
        long idProfesor,
        IFormFile archivo)
    {
        if (idProfesor <= 0)
        {
            return BadRequest(new
            {
                Mensaje = "El identificador del profesor no es válido."
            });
        }

        if (archivo == null || archivo.Length == 0)
        {
            return BadRequest(new
            {
                Mensaje = "Debe seleccionar una fotografía PNG."
            });
        }

        if (archivo.Length > 5 * 1024 * 1024)
        {
            return BadRequest(new
            {
                Mensaje = "La fotografía no puede superar los 5 MB."
            });
        }

        await using var stream = archivo.OpenReadStream();

        await _administradorService.CambiarFotoPerfilProfesorAsync(
            idProfesor,
            stream,
            archivo.FileName,
            archivo.ContentType);

        return Ok(new
        {
            Mensaje = "Fotografía del profesor actualizada correctamente."
        });
    }

    /// <summary>
    /// Elimina la fotografía de perfil de un profesor.
    /// Esta operación solo puede ser ejecutada por un Administrador.
    /// </summary>
    [HttpDelete("profesores/{idProfesor:long}/foto")]
    public async Task<IActionResult> EliminarFotoPerfilProfesor(
        long idProfesor)
    {
        if (idProfesor <= 0)
        {
            return BadRequest(new
            {
                Mensaje = "El identificador del profesor no es válido."
            });
        }

        await _administradorService.EliminarFotoPerfilProfesorAsync(
            idProfesor);

        return Ok(new
        {
            Mensaje = "Fotografía del profesor eliminada correctamente."
        });
    }

    /// <summary>
    /// Cambia la fotografía de perfil de un estudiante.
    /// Esta operación solo puede ser ejecutada por un Administrador.
    /// </summary>
    [HttpPut("estudiantes/{idEstudiante:long}/foto")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CambiarFotoPerfilEstudiante(
        long idEstudiante,
        IFormFile archivo)
    {
        if (idEstudiante <= 0)
        {
            return BadRequest(new
            {
                Mensaje = "El identificador del estudiante no es válido."
            });
        }

        if (archivo == null || archivo.Length == 0)
        {
            return BadRequest(new
            {
                Mensaje = "Debe seleccionar una fotografía PNG."
            });
        }

        if (archivo.Length > 5 * 1024 * 1024)
        {
            return BadRequest(new
            {
                Mensaje = "La fotografía no puede superar los 5 MB."
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
                Mensaje = "Formato de imagen no válido. Solo se permiten archivos PNG."
            });
        }

        await using var stream = archivo.OpenReadStream();

        await _administradorService.CambiarFotoPerfilEstudianteAsync(
            idEstudiante,
            stream,
            archivo.FileName,
            archivo.ContentType);

        return Ok(new
        {
            Mensaje = "Fotografía del estudiante actualizada correctamente."
        });
    }

    /// <summary>
    /// Elimina la fotografía de perfil de un estudiante.
    /// Esta operación solo puede ser ejecutada por un Administrador.
    /// </summary>
    [HttpDelete("estudiantes/{idEstudiante:long}/foto")]
    public async Task<IActionResult> EliminarFotoPerfilEstudiante(
        long idEstudiante)
    {
        if (idEstudiante <= 0)
        {
            return BadRequest(new
            {
                Mensaje = "El identificador del estudiante no es válido."
            });
        }

        await _administradorService.EliminarFotoPerfilEstudianteAsync(
            idEstudiante);

        return Ok(new
        {
            Mensaje = "Fotografía del estudiante eliminada correctamente."
        });
    }



    /// <summary>
    /// Edita un profesor.
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

    /// <summary>
    /// Cambia el estado de un estudiante.
    /// Esta operación solo puede ser ejecutada por un Administrador.
    /// </summary>
    [HttpPut("estudiantes/{idEstudiante:long}/estado")]
    public async Task<IActionResult> CambiarEstadoEstudiante(
        long idEstudiante,
        CambiarEstadoEstudianteRequestDto request)
    {
        await _administradorService.CambiarEstadoEstudianteAsync(
            idEstudiante,
            request);

        return Ok(new
        {
            Mensaje = "Estado del estudiante actualizado correctamente."
        });
    }
}
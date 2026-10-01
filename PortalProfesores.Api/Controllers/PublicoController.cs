using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalProfesores.Domain.Interfaces.Repositories;

namespace PortalProfesores.Api.Controllers;

/// <summary>
/// Expone información pública del portal.
/// </summary>
[ApiController]
[AllowAnonymous]
[Route("api/publico")]
public class PublicoController : ControllerBase
{
    private readonly IProfesorRepository _profesorRepository;

    public PublicoController(
        IProfesorRepository profesorRepository)
    {
        _profesorRepository = profesorRepository;
    }

    /// <summary>
    /// Obtiene los profesores activos
    /// disponibles para la Landing.
    /// </summary>
    [HttpGet("profesores")]
    public async Task<IActionResult> ObtenerProfesores()
    {
        var profesores =
            await _profesorRepository
                .ObtenerProfesoresPublicosAsync();

        return Ok(profesores);
    }
}
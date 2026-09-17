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
}
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalProfesores.Domain.Interfaces.Services;

namespace PortalProfesores.Api.Controllers;

/// <summary>
/// Endpoints relacionados con la gestión de usuarios.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    /// <summary>
    /// Obtiene el perfil del usuario autenticado.
    /// </summary>
    [HttpGet("perfil")]
    public async Task<IActionResult> ObtenerPerfil()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? User.FindFirst(ClaimTypes.Name)?.Value
                      ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrWhiteSpace(idClaim))
            return Unauthorized();

        var perfil = await _usuarioService.ObtenerPerfilAsync(long.Parse(idClaim));

        return Ok(perfil);
    }
}
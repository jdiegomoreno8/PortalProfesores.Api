
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalProfesores.Application.DTOs.Pagos;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Domain.Interfaces.Services;
using System.Security.Claims;

namespace PortalProfesores.Api.Controllers;

/// <summary>
/// Controlador para iniciar y consultar operaciones de pago.
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PagosController : ControllerBase
{
    private readonly IPagoService _pagoService;
    private readonly IUsuarioRepository _usuarioRepository;

    public PagosController(
        IPagoService pagoService,
        IUsuarioRepository usuarioRepository)
    {
        _pagoService = pagoService;
        _usuarioRepository = usuarioRepository;
    }

    /// <summary>
    /// Inicia el pago de una reserva para la persona autenticada.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> IniciarPago(
        [FromBody] IniciarPagoRequestDto request)
    {
        var idPersona = await ObtenerIdPersonaAutenticadaAsync();

        if (idPersona is null)
        {
            return Unauthorized(new
            {
                mensaje = "No fue posible identificar al usuario autenticado."
            });
        }

        try
        {
            var pago = await _pagoService.IniciarPagoAsync(
                request.IdReserva,
                idPersona.Value);

            return Ok(pago);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                mensaje = ex.Message
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                mensaje = ex.Message
            });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    /// <summary>
    /// Obtiene la persona asociada al usuario autenticado.
    /// </summary>
    private async Task<long?> ObtenerIdPersonaAutenticadaAsync()
    {
        var identificadorUsuario = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!long.TryParse(identificadorUsuario, out var idUsuario)
            || idUsuario <= 0)
        {
            return null;
        }

        var usuario = await _usuarioRepository
            .ObtenerPorIdAsync(idUsuario);

        if (usuario?.Persona is null)
        {
            return null;
        }

        return usuario.IdPersona;
    }
}

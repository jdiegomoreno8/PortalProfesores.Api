using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalProfesores.Domain.Interfaces.Services;
using System.Security.Claims;
using PortalProfesores.Domain.Interfaces.Repositories;


namespace PortalProfesores.Api.Controllers;

/// <summary>
/// Controlador para consultar y gestionar reservas.
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ReservaController : ControllerBase
{
    private readonly IReservaService _reservaService;
    private readonly IUsuarioRepository _usuarioRepository;

    public ReservaController(IReservaService reservaService, 
        IUsuarioRepository usuarioRepository )
    {
        _reservaService = reservaService;
        _usuarioRepository = usuarioRepository;
    }

    /// <summary>
    /// Obtiene el listado de reservas.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ObtenerListado()
    {
        var reservas = await _reservaService.ObtenerListadoAsync();

        return Ok(reservas);
    }


    /// <summary>
    /// Obtiene las disponibilidades que pueden reservarse.
    /// </summary>
    [HttpGet("disponibilidades")]
    public async Task<IActionResult> ObtenerDisponibilidadesDisponibles()
    {
        var disponibilidades = await _reservaService.ObtenerDisponiblesAsync();

        return Ok(disponibilidades);
    }


    /// <summary>
    /// Obtiene una reserva por su identificador.
    /// </summary>
    [HttpGet("{idReserva:long}")]
    public async Task<IActionResult> ObtenerPorId(long idReserva)
    {
        if (idReserva <= 0)
        {
            return BadRequest(new
            {
                mensaje = "El identificador de la reserva debe ser mayor que cero."
            });
        }

        var reserva = await _reservaService.ObtenerPorIdAsync(idReserva);

        if (reserva is null)
        {
            return NotFound(new
            {
                mensaje = "No se encontró la reserva solicitada."
            });
        }

        return Ok(reserva);
    }

    private async Task<long?> ObtenerIdPersonaAutenticadaAsync()
    {
        var identificadorUsuario = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!long.TryParse(identificadorUsuario, out var idUsuario)
            || idUsuario <= 0)
        {
            return null;
        }

        var usuario = await _usuarioRepository.ObtenerPorIdAsync(idUsuario);

        if (usuario?.Persona is null)
        {
            return null;
        }

        return usuario.IdPersona;
    }
    /// Crea una reserva para el usuario autenticado.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Crear(
        [FromBody] PortalProfesores.Domain.DTOs.Reservas.CrearReservaRequestDto request)
    {
        var idPersona = await ObtenerIdPersonaAutenticadaAsync();

        if (idPersona is null)
        {
            return Unauthorized(new
            {
                mensaje = "No fue posible identificar al usuario autenticado."
            });
        }

        request.IdPersona = idPersona.Value;

        try
        {
            var reserva = await _reservaService.CrearAsync(request);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { idReserva = reserva.IdReserva },
                reserva);
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
    }
}
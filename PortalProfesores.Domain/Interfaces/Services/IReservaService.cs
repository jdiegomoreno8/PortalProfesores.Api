using PortalProfesores.Domain.DTOs.Reservas;


namespace PortalProfesores.Domain.Interfaces.Services;

/// <summary>
/// Define las operaciones relacionadas
/// con la consulta y gestión de reservas.
/// </summary>
public interface IReservaService
{
    /// <summary>
    /// Obtiene el listado de reservas.
    /// </summary>
    Task<IEnumerable<ReservaListadoDto>> ObtenerListadoAsync();

    /// <summary>
    /// Obtiene una reserva por su identificador.
    /// </summary>
    Task<ReservaResponseDto?> ObtenerPorIdAsync(long idReserva);

    /// <summary>
    /// Crea una nueva reserva.
    /// </summary>
    Task<ReservaResponseDto> CrearAsync(
        CrearReservaRequestDto request);

    Task<ReservaResponseDto> EditarAsync(
    long idReserva,
    EditarReservaRequestDto request);

    /// <summary>
    /// Obtiene las disponibilidades que pueden reservarse.
    /// </summary>
    Task<IEnumerable<DisponibilidadDisponibleDto>> ObtenerDisponiblesAsync();
}
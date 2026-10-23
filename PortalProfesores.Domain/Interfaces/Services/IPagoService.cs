
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Domain.Interfaces.Services;

/// <summary>
/// Define las operaciones de negocio relacionadas con los pagos.
/// </summary>
public interface IPagoService
{
    /// <summary>
    /// Obtiene un pago por su identificador.
    /// </summary>
    Task<Pago?> ObtenerPorIdAsync(long idPago);

    /// <summary>
    /// Consulta el pago más reciente asociado a una reserva.
    /// </summary>
    Task<Pago?> ObtenerPorReservaAsync(long idReserva);

    Task<Pago> IniciarPagoAsync(
        long idReserva,
        long idPersona);
}

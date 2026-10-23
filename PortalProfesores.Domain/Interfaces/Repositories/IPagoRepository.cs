
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Domain.Interfaces.Repositories;

/// <summary>
/// Define las operaciones de persistencia para los pagos.
/// </summary>
public interface IPagoRepository
{
    /// <summary>
    /// Obtiene un pago por su identificador.
    /// </summary>
    Task<Pago?> ObtenerPorIdAsync(long idPago);

    /// <summary>
    /// Obtiene el pago asociado a una reserva.
    /// </summary>
    Task<Pago?> ObtenerPorReservaAsync(long idReserva);

    /// <summary>
    /// Registra un nuevo pago.
    /// </summary>
    Task<Pago> CrearAsync(Pago pago);

    /// <summary>
    /// Actualiza la información de un pago existente.
    /// </summary>
    Task<Pago> ActualizarAsync(Pago pago);

    /// <summary>
    /// Actualiza los datos de un pago sin guardar inmediatamente los cambios.
    /// El guardado queda bajo el control de la transacción del servicio.
    /// </summary>
    Task<Pago> ActualizarSinGuardarAsync(Pago pago);
}

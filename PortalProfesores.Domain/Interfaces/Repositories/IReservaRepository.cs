using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Domain.Interfaces.Repositories;

public interface IReservaRepository
{
    Task<IEnumerable<Reserva>> ObtenerListadoAsync();

    Task<Reserva?> ObtenerPorIdAsync(long idReserva);


    /// <summary>
    /// Crea una reserva y su sesión, y bloquea la disponibilidad
    /// dentro de una operación transaccional.
    /// </summary>
    Task<Reserva> CrearConBloqueoAsync(
        Reserva reserva,
        long idClase);


    /// <summary>
    /// Edita la disponibilidad de una reserva confirmada
    /// y cambia los horarios dentro de una transacción.
    /// </summary>
    Task<Reserva> EditarDisponibilidadConBloqueoAsync(
        long idReserva,
        long idNuevaDisponibilidad);

    /// <summary>
    /// Expira las reservas pendientes de pago cuyo plazo ha vencido
    /// y libera sus disponibilidades cuando corresponda.
    /// </summary>
    Task<int> ExpirarReservasVencidasAsync();

    /// <summary>
    /// Confirma una reserva pendiente de pago y marca su disponibilidad
    /// como reservada dentro de una transacción.
    /// </summary>
    Task ConfirmarReservaPagadaAsync(long idReserva);

}
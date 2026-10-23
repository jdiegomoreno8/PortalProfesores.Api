using Microsoft.EntityFrameworkCore;
using PortalProfesores.Domain.Entities;
using PortalProfesores.Domain.Exceptions;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Infrastructure.Data;
using System.Data;

namespace PortalProfesores.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de reservas.
/// </summary>
public class ReservaRepository : IReservaRepository
{
    private readonly PortalProfesoresDbContext _context;

    public ReservaRepository(PortalProfesoresDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene el listado de reservas registradas.
    /// </summary>
    public async Task<IEnumerable<Reserva>> ObtenerListadoAsync()
    {
        return await _context.Reservas
            .AsNoTracking()
            .Include(x => x.Disponibilidad)
                .ThenInclude(d => d!.Profesor)
                    .ThenInclude(p => p!.Persona)
            .Include(x => x.Persona)
            .Include(x => x.EstadoReserva)
            .OrderByDescending(x => x.FechaReserva)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene una reserva por su identificador.
    /// </summary>

    public async Task<Reserva?> ObtenerPorIdAsync(long idReserva)
    {
        return await _context.Reservas
            .AsNoTracking()
            .Include(r => r.Disponibilidad)
                .ThenInclude(d => d!.Profesor)
                    .ThenInclude(p => p!.Persona)
            .Include(r => r.Persona)
            .Include(r => r.EstadoReserva)
            .Include(r => r.Sesion)
                .ThenInclude(s => s!.Clase)
                    .ThenInclude(c => c!.Moneda)
            .FirstOrDefaultAsync(r => r.IdReserva == idReserva);
    }


    /// <summary>
    /// Crea una reserva y bloquea la disponibilidad
    /// dentro de una misma transacción.
    /// </summary>

    public async Task<Reserva> CrearConBloqueoAsync(
        Reserva reserva,
        long idClase)
    {
        ArgumentNullException.ThrowIfNull(reserva);

        if (idClase <= 0)
        {
            throw new ArgumentException(
                "Debe indicar una clase válida.",
                nameof(idClase));
        }

        await using var transaccion =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var estados = await _context.Set<EstadoDisponibilidad>()
                .AsNoTracking()
                .Where(e =>
                    e.Nombre == "Disponible" ||
                    e.Nombre == "Bloqueada temporalmente")
                .ToListAsync();

            var estadoDisponible = estados
                .SingleOrDefault(e => e.Nombre == "Disponible");

            var estadoBloqueado = estados
                .SingleOrDefault(e =>
                    e.Nombre == "Bloqueada temporalmente");

            if (estadoDisponible == null || estadoBloqueado == null)
            {
                throw new InvalidOperationException(
                    "No se encontraron los estados requeridos " +
                    "en el catálogo de disponibilidad.");
            }

            // Bloquear el horario solo si continúa disponible.
            var filasActualizadas = await _context.Disponibilidades
                .Where(d =>
                    d.IdDisponibilidad == reserva.IdDisponibilidad &&
                    d.IdEstadoDisponibilidad ==
                        estadoDisponible.IdEstadoDisponibilidad)
                .ExecuteUpdateAsync(actualizacion =>
                    actualizacion.SetProperty(
                        d => d.IdEstadoDisponibilidad,
                        estadoBloqueado.IdEstadoDisponibilidad));

            if (filasActualizadas == 0)
            {
                throw new ConflictException(
               "Este horario está siendo gestionado por otra persona. Inténtalo nuevamente más tarde.");
            }

            // Crear la reserva para obtener su identificador.
            await _context.Reservas.AddAsync(reserva);
            await _context.SaveChangesAsync();

            // Crear la sesión asociada a la reserva y a la clase.
            var sesion = new Sesion
            {
                IdReserva = reserva.IdReserva,
                IdClase = idClase,
                FechaCreacion = DateTime.UtcNow
            };

            await _context.Sesiones.AddAsync(sesion);
            await _context.SaveChangesAsync();

            // Confirmar las tres operaciones juntas.
            await transaccion.CommitAsync();

            return reserva;
        }
        catch (DbUpdateException ex)
        {
            await transaccion.RollbackAsync();

            _context.Entry(reserva).State = EntityState.Detached;

            var existeReserva = await _context.Reservas
                .AsNoTracking()
                .AnyAsync(r =>
                    r.IdDisponibilidad == reserva.IdDisponibilidad);

            if (existeReserva)
            {
                throw new InvalidOperationException(
                    "La disponibilidad seleccionada acaba de ser reservada.",
                    ex);
            }

            throw;
        }
        catch
        {
            await transaccion.RollbackAsync();

            if (_context.Entry(reserva).State != EntityState.Detached)
            {
                _context.Entry(reserva).State = EntityState.Detached;
            }

            throw;
        }
    }


    /// <summary>
    /// Cambia la disponibilidad de una reserva dentro de una transacción.
    /// El horario anterior solo se libera si el nuevo se puede reservar.
    /// </summary>
    public async Task<Reserva> EditarDisponibilidadConBloqueoAsync(
        long idReserva,
        long idNuevaDisponibilidad)
    {
        await using var transaccion =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var reserva = await _context.Reservas
                .FirstOrDefaultAsync(r => r.IdReserva == idReserva);

            if (reserva == null)
            {
                throw new KeyNotFoundException(
                    "No se encontró la reserva solicitada.");
            }

            if (reserva.IdDisponibilidad == idNuevaDisponibilidad)
            {
                throw new InvalidOperationException(
                    "Debes seleccionar una disponibilidad diferente.");
            }

            var estados = await _context.Set<EstadoDisponibilidad>()
                .AsNoTracking()
                .Where(e =>
                    e.Nombre == "Disponible" ||
                    e.Nombre == "Bloqueada temporalmente" ||
                    e.Nombre == "Reservada")
                .ToListAsync();

            var estadoDisponible = estados.SingleOrDefault(
                e => e.Nombre == "Disponible");

            var estadoBloqueado = estados.SingleOrDefault(
                e => e.Nombre == "Bloqueada temporalmente");

            var estadoReservado = estados.SingleOrDefault(
                e => e.Nombre == "Reservada");

            if (estadoDisponible == null ||
                estadoBloqueado == null ||
                estadoReservado == null)
            {
                throw new InvalidOperationException(
                    "No se encontraron todos los estados requeridos " +
                    "en el catálogo de disponibilidad.");
            }

            // Primero bloqueamos el nuevo horario.
            // Solo se actualiza si sigue disponible.
            var filasNuevoHorario = await _context.Disponibilidades
                .Where(d =>
                    d.IdDisponibilidad == idNuevaDisponibilidad &&
                    d.IdEstadoDisponibilidad ==
                        estadoDisponible.IdEstadoDisponibilidad)
                .ExecuteUpdateAsync(actualizacion =>
                    actualizacion.SetProperty(
                        d => d.IdEstadoDisponibilidad,
                        estadoBloqueado.IdEstadoDisponibilidad));

            if (filasNuevoHorario == 0)
            {
                throw new InvalidOperationException(
                    "El nuevo horario ya no está disponible.");
            }

            // Liberamos el horario anterior únicamente si está reservado
            // o bloqueado temporalmente.
            var filasHorarioAnterior = await _context.Disponibilidades
                .Where(d =>
                    d.IdDisponibilidad == reserva.IdDisponibilidad &&
                    (d.IdEstadoDisponibilidad ==
                        estadoReservado.IdEstadoDisponibilidad ||
                     d.IdEstadoDisponibilidad ==
                        estadoBloqueado.IdEstadoDisponibilidad))
                .ExecuteUpdateAsync(actualizacion =>
                    actualizacion.SetProperty(
                        d => d.IdEstadoDisponibilidad,
                        estadoDisponible.IdEstadoDisponibilidad));

            if (filasHorarioAnterior == 0)
            {
                throw new InvalidOperationException(
                    "No se pudo liberar el horario anterior. " +
                    "La reserva no ha sido modificada.");
            }

            reserva.IdDisponibilidad = idNuevaDisponibilidad;
            reserva.FechaActualizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await transaccion.CommitAsync();

            return reserva;
        }
        catch
        {
            await transaccion.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// Expira las reservas pendientes de pago cuyo plazo ha vencido.
    /// Libera la disponibilidad únicamente cuando no existen otras
    /// reservas activas que utilicen el mismo horario.
    /// </summary>
    public async Task<int> ExpirarReservasVencidasAsync()
    {
        var ahora = DateTime.UtcNow;

        await using var transaccion =
            await _context.Database.BeginTransactionAsync(
                IsolationLevel.Serializable);

        try
        {
            // Consultar los estados necesarios desde los catálogos.
            var estadosReserva = await _context.Set<EstadoReserva>()
                .AsNoTracking()
                .Where(e =>
                    e.Nombre == "PendientePago" ||
                    e.Nombre == "Confirmada" ||
                    e.Nombre == "Cancelada")
                .ToListAsync();

            var estadoPendiente = estadosReserva
                .SingleOrDefault(e => e.Nombre == "PendientePago");

            var estadoConfirmada = estadosReserva
                .SingleOrDefault(e => e.Nombre == "Confirmada");

            var estadoCancelada = estadosReserva
                .SingleOrDefault(e => e.Nombre == "Cancelada");

            if (estadoPendiente == null ||
                estadoConfirmada == null ||
                estadoCancelada == null)
            {
                throw new InvalidOperationException(
                    "No se encontraron los estados requeridos " +
                    "en el catálogo de reservas.");
            }


            var estadosDisponibilidad =
                await _context.Set<EstadoDisponibilidad>()
                    .AsNoTracking()
                    .Where(e =>
                        e.Nombre == "Disponible" ||
                        e.Nombre == "Bloqueada temporalmente")
                    .ToListAsync();

            var estadoDisponible = estadosDisponibilidad
                .SingleOrDefault(e => e.Nombre == "Disponible");

            var estadoBloqueado = estadosDisponibilidad
                .SingleOrDefault(e => e.Nombre == "Bloqueada temporalmente");

            if (estadoDisponible == null || estadoBloqueado == null)
            {
                throw new InvalidOperationException(
                    "No se encontraron los estados requeridos " +
                    "en el catálogo de disponibilidad.");
            }


            var estadoPagoAprobado = await _context.Set<EstadoPago>()
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.Nombre == "Aprobado");

            if (estadoPagoAprobado == null)
            {
                throw new InvalidOperationException(
                    "No se encontró el estado Aprobado en el catálogo de pagos.");
            }



            // Buscar únicamente reservas pendientes con expiración vencida.

            var reservasVencidas = await _context.Reservas
                .AsNoTracking()
                .Where(r =>
                    r.IdEstadoReserva == estadoPendiente.IdEstadoReserva &&
                    r.FechaExpiracion.HasValue &&
                    r.FechaExpiracion.Value <= ahora &&
                    !_context.Set<Pago>().Any(p =>
                        p.IdReserva == r.IdReserva &&
                        p.IdEstadoPago == estadoPagoAprobado.IdEstadoPago))
                .Select(r => new
                {
                    r.IdReserva,
                    r.IdDisponibilidad
                })
                .ToListAsync();


            var cantidadExpirada = 0;

            // Procesar cada reserva con una actualización condicional.
            foreach (var reserva in reservasVencidas)
            {
                var filasReserva = await _context.Reservas
                    .Where(r =>
                        r.IdReserva == reserva.IdReserva &&
                        r.IdEstadoReserva ==
                            estadoPendiente.IdEstadoReserva &&
                        r.FechaExpiracion.HasValue &&
                        r.FechaExpiracion.Value <= ahora)
                    .ExecuteUpdateAsync(actualizacion =>
                        actualizacion
                            .SetProperty(
                                r => r.IdEstadoReserva,
                                estadoCancelada.IdEstadoReserva)
                            .SetProperty(
                                r => r.FechaActualizacion,
                                ahora));

                // Si cambió el estado antes de nuestra actualización,
                // no debemos liberar su disponibilidad.
                if (filasReserva == 0)
                {
                    continue;
                }

                cantidadExpirada++;

                // Liberar solo si sigue bloqueada temporalmente
                // y no existe otra reserva pendiente o confirmada.
                await _context.Disponibilidades
                    .Where(d =>
                        d.IdDisponibilidad == reserva.IdDisponibilidad &&
                       d.IdEstadoDisponibilidad == estadoBloqueado.IdEstadoDisponibilidad &&
                        !_context.Reservas.Any(otra =>
                            otra.IdDisponibilidad == d.IdDisponibilidad &&
                            (otra.IdEstadoReserva ==
                                estadoPendiente.IdEstadoReserva ||
                             otra.IdEstadoReserva ==
                                estadoConfirmada.IdEstadoReserva)))
                    .ExecuteUpdateAsync(actualizacion =>
    actualizacion.SetProperty(
        d => d.IdEstadoDisponibilidad,
        estadoDisponible.IdEstadoDisponibilidad));
            }

            await transaccion.CommitAsync();

            return cantidadExpirada;
        }
        catch
        {
            await transaccion.RollbackAsync();
            throw;
        }


    }


    public async Task ConfirmarReservaPagadaAsync(long idReserva)
    {
        if (idReserva <= 0)
            throw new ArgumentException(
                "Debe indicar una reserva válida.",
                nameof(idReserva));

        var ahora = DateTime.UtcNow;

        var estadoPendiente = await _context.EstadosReserva
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Nombre == "PendientePago");

        var estadoConfirmada = await _context.EstadosReserva
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Nombre == "Confirmada");

        var estadoBloqueada = await _context.EstadosDisponibilidad
            .AsNoTracking()
            .FirstOrDefaultAsync(e =>
                e.Nombre == "Bloqueada temporalmente");

        var estadoReservada = await _context.EstadosDisponibilidad
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Nombre == "Reservada");

        if (estadoPendiente is null ||
            estadoConfirmada is null ||
            estadoBloqueada is null ||
            estadoReservada is null)
        {
            throw new InvalidOperationException(
                "No se encontraron los estados necesarios para confirmar la reserva.");
        }

        var reserva = await _context.Reservas
            .AsNoTracking()
            .FirstOrDefaultAsync(r =>
                r.IdReserva == idReserva &&
                r.IdEstadoReserva == estadoPendiente.IdEstadoReserva &&
                r.FechaExpiracion.HasValue &&
                r.FechaExpiracion.Value > ahora);

        if (reserva is null)
        {
            throw new InvalidOperationException(
                "La reserva ya no está pendiente de pago o ha vencido.");
        }

        var reservaActualizada = await _context.Reservas
            .Where(r =>
                r.IdReserva == idReserva &&
                r.IdEstadoReserva == estadoPendiente.IdEstadoReserva &&
                r.FechaExpiracion.HasValue &&
                r.FechaExpiracion.Value > ahora)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(
                    r => r.IdEstadoReserva,
                    estadoConfirmada.IdEstadoReserva)
                .SetProperty(
                    r => r.FechaActualizacion,
                    ahora));

        if (reservaActualizada == 0)
        {
            throw new InvalidOperationException(
                "La reserva cambió de estado o venció durante la confirmación.");
        }

        var disponibilidadActualizada = await _context.Disponibilidades
            .Where(d =>
                d.IdDisponibilidad == reserva.IdDisponibilidad &&
                d.IdEstadoDisponibilidad ==
                    estadoBloqueada.IdEstadoDisponibilidad)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(
                    d => d.IdEstadoDisponibilidad,
                    estadoReservada.IdEstadoDisponibilidad));

        if (disponibilidadActualizada == 0)
        {
            throw new InvalidOperationException(
                "La disponibilidad ya no está bloqueada para esta reserva.");
        }
    }


}
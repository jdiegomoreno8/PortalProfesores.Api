using PortalProfesores.Domain.DTOs.Reservas;
using PortalProfesores.Domain.Entities;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Domain.Interfaces.Services;
using PortalProfesores.Domain.DTOs.Reservas;
using Microsoft.EntityFrameworkCore;
using PortalProfesores.Infrastructure.Data;
using PortalProfesores.Domain.Exceptions;

namespace PortalProfesores.Infrastructure.Services;

/// <summary>
/// Implementa la lógica de negocio relacionada
/// con la consulta y creación de reservas.
/// </summary>
public class ReservaService : IReservaService
{
    private readonly IReservaRepository _reservaRepository;
    private readonly IDisponibilidadRepository _disponibilidadRepository;
    private readonly IClaseRepository _claseRepository;
    private readonly IConfiguracionService _configuracionService;
    private readonly PortalProfesoresDbContext _context;
    

    public ReservaService(
        IReservaRepository reservaRepository,
        IDisponibilidadRepository disponibilidadRepository,
         IClaseRepository claseRepository,
         IConfiguracionService configuracionService,
        PortalProfesoresDbContext context)
    {
        _reservaRepository = reservaRepository;
        _disponibilidadRepository = disponibilidadRepository;
        _claseRepository = claseRepository;
        _configuracionService = configuracionService;
        _context = context;
    }

    /// <summary>
    /// Obtiene el listado de reservas.
    /// </summary>

    public async Task<IEnumerable<ReservaListadoDto>> ObtenerListadoAsync()
{
    var reservas = await _reservaRepository.ObtenerListadoAsync();

    return reservas.Select(reserva => new ReservaListadoDto
    {
        IdReserva = reserva.IdReserva,
        IdDisponibilidad = reserva.IdDisponibilidad,
        IdPersona = reserva.IdPersona,

        NombreEstudiante = string.Join(" ",
            new[]
            {
                reserva.Persona?.Nombres,
                reserva.Persona?.Apellidos
            }.Where(nombre => !string.IsNullOrWhiteSpace(nombre))),

        NombreProfesor = string.Join(" ",
            new[]
            {
                reserva.Disponibilidad?.Profesor?.Persona?.Nombres,
                reserva.Disponibilidad?.Profesor?.Persona?.Apellidos
            }.Where(nombre => !string.IsNullOrWhiteSpace(nombre))),

        FechaClase = reserva.Disponibilidad?.Fecha ?? DateTime.MinValue,
        HoraInicio = reserva.Disponibilidad?.HoraInicio ?? TimeSpan.Zero,
        HoraFin = reserva.Disponibilidad?.HoraFin ?? TimeSpan.Zero,

        EstadoReserva = reserva.EstadoReserva?.Nombre ?? string.Empty,
        FechaReserva = reserva.FechaReserva
    });
}


    /// <summary>
    /// Obtiene una reserva por su identificador.
    /// </summary>
    public async Task<ReservaResponseDto?> ObtenerPorIdAsync(long idReserva)
    {
        if (idReserva <= 0)
        {
            throw new ArgumentException(
                "El identificador de la reserva debe ser mayor que cero.",
                nameof(idReserva));
        }

        var reserva = await _reservaRepository.ObtenerPorIdAsync(idReserva);

        if (reserva is null)
        {
            return null;
        }

        var disponibilidad = reserva.Disponibilidad;

        return new ReservaResponseDto
        {
            IdReserva = reserva.IdReserva,
            IdDisponibilidad = reserva.IdDisponibilidad,
            IdPersona = reserva.IdPersona,

            NombreEstudiante = reserva.Persona is not null
                ? $"{reserva.Persona.Nombres} {reserva.Persona.Apellidos}".Trim()
                : string.Empty,

            NombreProfesor = disponibilidad?.Profesor?.Persona is not null
                ? $"{disponibilidad.Profesor.Persona.Nombres} {disponibilidad.Profesor.Persona.Apellidos}".Trim()
                : string.Empty,

            FechaClase = disponibilidad?.Fecha ?? DateTime.MinValue,
            HoraInicio = disponibilidad?.HoraInicio ?? TimeSpan.Zero,
            HoraFin = disponibilidad?.HoraFin ?? TimeSpan.Zero,

            EstadoReserva = reserva.EstadoReserva?.Nombre ?? string.Empty,
            FechaReserva = reserva.FechaReserva,
            Observacion = reserva.Observacion
        };
    }

    /// <summary>
    /// Obtiene las disponibilidades que pueden reservarse.
    /// </summary>
    public async Task<IEnumerable<DisponibilidadDisponibleDto>> ObtenerDisponiblesAsync()
    {
        return await _disponibilidadRepository.ObtenerDisponiblesAsync();
    }

    /// <summary>
    /// Crea una reserva en estado PendientePago
    /// si la disponibilidad existe y está libre.
    /// </summary>
    public async Task<ReservaResponseDto> CrearAsync(
        CrearReservaRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.IdDisponibilidad <= 0)
        {
            throw new ArgumentException(
                "Debe indicar una disponibilidad válida.",
                nameof(request));
        }

        if (request.IdPersona <= 0)
        {
            throw new ArgumentException(
                "Debe indicar una persona válida.",
                nameof(request));
        }

        var disponibilidad = await _disponibilidadRepository
            .ObtenerPorIdAsync(request.IdDisponibilidad);

        if (disponibilidad is null)
        {
            throw new KeyNotFoundException(
                "La disponibilidad indicada no existe.");
        }

        if (!string.Equals(
       disponibilidad.EstadoDisponibilidad?.Nombre,
       "Disponible",
       StringComparison.OrdinalIgnoreCase))
        {
            throw new ConflictException(
                "Este horario está siendo gestionado por otra persona. Inténtalo nuevamente más tarde.");
        }
        /// <summary>
        /// Validar la clase antes de crear la reserva
        /// </summary>
        if (request.IdClase <= 0)
        {
            throw new ArgumentException(
                "Debe indicar una clase válida.",
                nameof(request));
        }

        var clase = await _claseRepository
            .ObtenerPorIdAsync(request.IdClase);

        if (clase is null)
        {
            throw new KeyNotFoundException(
                "La clase indicada no existe.");
        }

        if (clase.IdEstadoClase != 1)
        {
            throw new InvalidOperationException(
                "La clase seleccionada no está activa.");
        }

        var idProfesor = disponibilidad.IdProfesor;

        var profesorPuedeImpartir = await _claseRepository
            .ProfesorPuedeImpartirClaseAsync(
                idProfesor,
                request.IdClase);

        if (!profesorPuedeImpartir)
        {
            throw new InvalidOperationException(
                "El profesor seleccionado no está asignado para impartir esta clase.");
        }
        /// <summary>
        /// Zona Horario
        /// </summary>


        var codigoZonaHoraria = disponibilidad.ZonaHoraria?.Codigo;

        if (string.IsNullOrWhiteSpace(codigoZonaHoraria))
        {
            throw new InvalidOperationException(
                "La disponibilidad no tiene una zona horaria válida.");
        }

        TimeZoneInfo zonaHoraria;

        try
        {
            zonaHoraria = TimeZoneInfo.FindSystemTimeZoneById(
                codigoZonaHoraria);
        }
        catch (TimeZoneNotFoundException)
        {
            throw new InvalidOperationException(
                $"No se reconoce la zona horaria '{codigoZonaHoraria}'.");
        }
        catch (InvalidTimeZoneException)
        {
            throw new InvalidOperationException(
                $"La zona horaria '{codigoZonaHoraria}' no es válida.");
        }

        var fechaHoraLocal = DateTime.SpecifyKind(
            disponibilidad.Fecha.Date.Add(disponibilidad.HoraInicio),
            DateTimeKind.Unspecified);

        if (zonaHoraria.IsInvalidTime(fechaHoraLocal))
        {
            throw new InvalidOperationException(
                "La hora seleccionada no existe en esa zona horaria.");
        }

        if (zonaHoraria.IsAmbiguousTime(fechaHoraLocal))
        {
            throw new InvalidOperationException(
                "La hora seleccionada es ambigua por un cambio de horario. " +
                "Selecciona otro horario.");
        }

        var fechaHoraUtc = TimeZoneInfo.ConvertTimeToUtc(
            fechaHoraLocal,
            zonaHoraria);

        if (fechaHoraUtc <= DateTime.UtcNow.AddHours(2))
        {
            throw new InvalidOperationException(
                "La reserva debe realizarse con al menos dos horas de anticipación.");
        }

        var minutosExpiracion =
       await _configuracionService.ObtenerEnteroPositivoAsync(
           "TIEMPO_BLOQUEO_PAGO");

        var fechaCreacionReserva = DateTime.UtcNow;

        var reserva = new Reserva
        {
            IdDisponibilidad = request.IdDisponibilidad,
            IdPersona = request.IdPersona!.Value,
            IdEstadoReserva = 1,
            FechaReserva = fechaCreacionReserva,
            FechaExpiracion = fechaCreacionReserva.AddMinutes(minutosExpiracion),
            Observacion = request.Observacion
        };

        var creada = await _reservaRepository
           .CrearConBloqueoAsync(reserva, request.IdClase);

        // Consultar nuevamente la reserva con sus datos relacionados.
        var reservaCompleta = await _reservaRepository
            .ObtenerPorIdAsync(creada.IdReserva);

        if (reservaCompleta is null)
        {
            throw new InvalidOperationException(
                "La reserva se creó, pero no fue posible recuperar sus datos.");
        }

        var disponibilidadCreada = reservaCompleta.Disponibilidad;

        var personaEstudiante = reservaCompleta.Persona;
        var personaProfesor = disponibilidadCreada?.Profesor?.Persona;

        return new ReservaResponseDto
        {
            IdReserva = reservaCompleta.IdReserva,
            IdDisponibilidad = reservaCompleta.IdDisponibilidad,
            IdPersona = reservaCompleta.IdPersona,

            NombreEstudiante = personaEstudiante is not null
                ? $"{personaEstudiante.Nombres} {personaEstudiante.Apellidos}".Trim()
                : string.Empty,

            NombreProfesor = personaProfesor is not null
                ? $"{personaProfesor.Nombres} {personaProfesor.Apellidos}".Trim()
                : string.Empty,

            FechaClase = disponibilidadCreada?.Fecha ?? DateTime.MinValue,
            HoraInicio = disponibilidadCreada?.HoraInicio ?? TimeSpan.Zero,
            HoraFin = disponibilidadCreada?.HoraFin ?? TimeSpan.Zero,

            EstadoReserva = reservaCompleta.EstadoReserva?.Nombre
                ?? "PendientePago",

            FechaReserva = reservaCompleta.FechaReserva,
            FechaExpiracion = reservaCompleta.FechaExpiracion,
            Observacion = reservaCompleta.Observacion
        };

    }

    /// <summary>
    /// Edición de la reserva.
    /// </summary>
    public async Task<ReservaResponseDto> EditarAsync(
    long idReserva,
    EditarReservaRequestDto request)
    {
        if (idReserva <= 0)
        {
            throw new ArgumentException(
                "El identificador de la reserva no es válido.");
        }

        if (request.IdDisponibilidad <= 0)
        {
            throw new ArgumentException(
                "Debes seleccionar una disponibilidad válida.");
        }

        var reservaExistente = await _reservaRepository
    .ObtenerPorIdAsync(idReserva);

        if (reservaExistente is null)
        {
            throw new KeyNotFoundException(
                "No se encontró la reserva solicitada.");
        }

        if (reservaExistente.IdEstadoReserva != 2)
        {
            throw new InvalidOperationException(
                "Solo se pueden editar reservas confirmadas.");
        }

        var tienePagoAprobado = await _context.Pagos
            .AsNoTracking()
            .AnyAsync(p =>
                p.IdReserva == idReserva &&
                p.IdEstadoPago == 3);

        if (!tienePagoAprobado)
        {
            throw new InvalidOperationException(
                "Solo se pueden editar reservas con un pago aprobado.");
        }

        var nuevaDisponibilidad = await _disponibilidadRepository
    .ObtenerPorIdAsync(request.IdDisponibilidad);

        if (nuevaDisponibilidad is null)
        {
            throw new KeyNotFoundException(
                "La nueva disponibilidad no existe.");
        }

        var codigoZonaHoraria = nuevaDisponibilidad.ZonaHoraria?.Codigo;

        if (string.IsNullOrWhiteSpace(codigoZonaHoraria))
        {
            throw new InvalidOperationException(
                "La nueva disponibilidad no tiene una zona horaria válida.");
        }

        TimeZoneInfo zonaHoraria;

        try
        {
            zonaHoraria = TimeZoneInfo.FindSystemTimeZoneById(
                codigoZonaHoraria);
        }
        catch (TimeZoneNotFoundException)
        {
            throw new InvalidOperationException(
                $"No se reconoce la zona horaria '{codigoZonaHoraria}'.");
        }
        catch (InvalidTimeZoneException)
        {
            throw new InvalidOperationException(
                $"La zona horaria '{codigoZonaHoraria}' no es válida.");
        }

        var fechaHoraLocal = DateTime.SpecifyKind(
            nuevaDisponibilidad.Fecha.Date.Add(nuevaDisponibilidad.HoraInicio),
            DateTimeKind.Unspecified);

        if (zonaHoraria.IsInvalidTime(fechaHoraLocal))
        {
            throw new InvalidOperationException(
                "La hora seleccionada no existe en esa zona horaria.");
        }

        if (zonaHoraria.IsAmbiguousTime(fechaHoraLocal))
        {
            throw new InvalidOperationException(
                "La hora seleccionada es ambigua por un cambio de horario.");
        }

        var fechaHoraUtc = TimeZoneInfo.ConvertTimeToUtc(
            fechaHoraLocal,
            zonaHoraria);

        if (fechaHoraUtc <= DateTime.UtcNow.AddHours(2))
        {
            throw new InvalidOperationException(
                "El cambio debe realizarse con al menos dos horas de anticipación.");
        }

        if (nuevaDisponibilidad.EstadoDisponibilidad?.Nombre != "Disponible")
        {
            throw new InvalidOperationException(
                "La disponibilidad seleccionada ya no está disponible.");
        }

        var reserva = await _reservaRepository
            .EditarDisponibilidadConBloqueoAsync(
                idReserva,
                request.IdDisponibilidad);

        var reservaActualizada = await _reservaRepository
            .ObtenerPorIdAsync(reserva.IdReserva);

        if (reservaActualizada == null)
        {
            throw new KeyNotFoundException(
                "No se encontró la reserva después de actualizarla.");
        }

        return new ReservaResponseDto
        {
            IdReserva = reservaActualizada.IdReserva,
            IdDisponibilidad = reservaActualizada.IdDisponibilidad,
            IdPersona = reservaActualizada.IdPersona,
            NombreEstudiante =
                reservaActualizada.Persona?.Nombres ?? string.Empty,
            NombreProfesor =
                reservaActualizada.Disponibilidad?.Profesor?.Persona?.Nombres
                ?? string.Empty,
            FechaClase = reservaActualizada.Disponibilidad!.Fecha,
            HoraInicio = reservaActualizada.Disponibilidad.HoraInicio,
            HoraFin = reservaActualizada.Disponibilidad.HoraFin,
            EstadoReserva =
                reservaActualizada.EstadoReserva?.Nombre ?? string.Empty,
            FechaReserva = reservaActualizada.FechaReserva,
            Observacion = reservaActualizada.Observacion
        };
    }

}
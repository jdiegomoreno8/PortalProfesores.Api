namespace PortalProfesores.Domain.DTOs.Reservas;

/// <summary>
/// Información de una reserva para las respuestas de la API.
/// </summary>
public class ReservaResponseDto
{
    /// <summary>
    /// Identificador de la reserva.
    /// </summary>
    public long IdReserva { get; set; }

    /// <summary>
    /// Identificador de la disponibilidad reservada.
    /// </summary>
    public long IdDisponibilidad { get; set; }

    /// <summary>
    /// Identificador de la persona que solicita la reserva.
    /// </summary>
    public long IdPersona { get; set; }

    /// <summary>
    /// Nombre completo del estudiante.
    /// </summary>
    public string NombreEstudiante { get; set; } = string.Empty;

    /// <summary>
    /// Nombre completo del profesor.
    /// </summary>
    public string NombreProfesor { get; set; } = string.Empty;

    /// <summary>
    /// Fecha programada de la clase.
    /// </summary>
    public DateTime FechaClase { get; set; }

    /// <summary>
    /// Hora de inicio de la clase.
    /// </summary>
    public TimeSpan HoraInicio { get; set; }

    /// <summary>
    /// Hora de finalización de la clase.
    /// </summary>
    public TimeSpan HoraFin { get; set; }

    /// <summary>
    /// Nombre del estado actual de la reserva.
    /// </summary>
    public string EstadoReserva { get; set; } = string.Empty;

    /// <summary>
    /// Fecha y hora en que se registró la reserva, en UTC.
    /// </summary>
    public DateTime FechaReserva { get; set; }

    public DateTime? FechaExpiracion { get; set; }

    /// <summary>
    /// Observación de la reserva, si existe.
    /// </summary>
    public string? Observacion { get; set; }
}
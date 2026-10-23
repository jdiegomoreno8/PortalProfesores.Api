
namespace PortalProfesores.Domain.DTOs.Reservas;

/// <summary>
/// Información resumida de una reserva para listados.
/// </summary>
public class ReservaListadoDto
{
    /// <summary>
    /// Identificador de la reserva.
    /// </summary>
    public long IdReserva { get; set; }

    /// <summary>
    /// Identificador de la disponibilidad.
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
    /// Fecha de la clase.
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
    /// Estado actual de la reserva.
    /// </summary>
    public string EstadoReserva { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de registro de la reserva.
    /// </summary>
    public DateTime FechaReserva { get; set; }
}

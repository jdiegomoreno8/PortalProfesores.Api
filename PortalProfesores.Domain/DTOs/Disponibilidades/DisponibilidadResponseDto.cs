namespace PortalProfesores.Domain.DTOs.Disponibilidades;

/// <summary>
/// Información de una disponibilidad para las respuestas de la API.
/// </summary>
public class DisponibilidadResponseDto
{
    /// <summary>
    /// Identificador de la disponibilidad.
    /// </summary>
    public long IdDisponibilidad { get; set; }

    /// <summary>
    /// Identificador del profesor.
    /// </summary>
    public long IdProfesor { get; set; }

    /// <summary>
    /// Estado actual de la disponibilidad.
    /// </summary>
    public string EstadoDisponibilidad { get; set; } = string.Empty;

    /// <summary>
    /// Fecha del horario.
    /// </summary>
    public DateTime Fecha { get; set; }

    /// <summary>
    /// Hora de inicio del horario.
    /// </summary>
    public TimeSpan HoraInicio { get; set; }

    /// <summary>
    /// Hora de finalización del horario.
    /// </summary>
    public TimeSpan HoraFin { get; set; }

    /// <summary>
    /// Zona horaria asociada.
    /// </summary>
    public long IdZonaHoraria { get; set; }

    /// <summary>
    /// Observación opcional.
    /// </summary>
    public string? Observacion { get; set; }
}
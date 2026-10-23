
namespace PortalProfesores.Domain.Entities;

/// <summary>
/// Representa un bloque horario de disponibilidad de un profesor.
/// </summary>
public class Disponibilidad
{
    public long IdDisponibilidad { get; set; }

    public long IdProfesor { get; set; }

    public long IdEstadoDisponibilidad { get; set; }

    public long IdZonaHoraria { get; set; }

    public DateTime Fecha { get; set; }

    public TimeSpan HoraInicio { get; set; }

    public TimeSpan HoraFin { get; set; }

    public string? Observacion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public long? UsuarioActualizacion { get; set; }

    // Relaciones
    public ProfesorDetalle? Profesor { get; set; }

    public EstadoDisponibilidad? EstadoDisponibilidad { get; set; }

    public ZonaHoraria? ZonaHoraria { get; set; }

    
}

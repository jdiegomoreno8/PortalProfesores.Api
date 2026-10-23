
namespace PortalProfesores.Domain.DTOs.Reservas;

/// <summary>
/// Información de una disponibilidad que puede reservarse.
/// </summary>
public class DisponibilidadDisponibleDto
{
    public long IdDisponibilidad { get; set; }

    public DateTime Fecha { get; set; }

    public TimeSpan HoraInicio { get; set; }

    public TimeSpan HoraFin { get; set; }

    public string NombreProfesor { get; set; } = string.Empty;
}

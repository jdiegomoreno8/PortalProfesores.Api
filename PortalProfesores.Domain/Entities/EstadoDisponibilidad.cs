namespace PortalProfesores.Domain.Entities;

/// <summary>
/// Representa los estados posibles de un bloque de disponibilidad.
/// </summary>
public class EstadoDisponibilidad
{
    public long IdEstadoDisponibilidad { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public long? UsuarioActualizacion { get; set; }

    // Relaciones
    public ICollection<Disponibilidad> Disponibilidades { get; set; }
        = new List<Disponibilidad>();
}
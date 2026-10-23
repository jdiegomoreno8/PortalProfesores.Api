namespace PortalProfesores.Domain.Entities;

/// <summary>
/// Representa una zona horaria disponible para gestionar
/// la disponibilidad de los profesores.
/// </summary>
public class ZonaHoraria
{
    public long IdZonaHoraria { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Codigo { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public long? UsuarioActualizacion { get; set; }

    // Relaciones
    public ICollection<Disponibilidad> Disponibilidades { get; set; }
        = new List<Disponibilidad>();
}
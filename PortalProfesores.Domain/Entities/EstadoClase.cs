using System.ComponentModel.DataAnnotations;

namespace PortalProfesores.Domain.Entities;

/// <summary>
/// Representa el estado de una clase.
/// </summary>
public class EstadoClase
{
    [Key]
    public long IdEstadoClase { get; set; }

    [Required]
    [MaxLength(50)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Descripcion { get; set; }

    // Relaciones

    public ICollection<Clase> Clases { get; set; }
        = new List<Clase>();
}
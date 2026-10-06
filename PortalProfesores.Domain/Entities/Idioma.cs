using System.ComponentModel.DataAnnotations;

namespace PortalProfesores.Domain.Entities;

/// <summary>
/// Representa un idioma disponible en el portal.
/// </summary>
public class Idioma
{
    [Key]
    public long IdIdioma { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string CodigoISO { get; set; } = string.Empty;

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public long? UsuarioActualizacion { get; set; }


    // Relaciones

    public ICollection<Clase> Clases { get; set; }
        = new List<Clase>();
}
using System.ComponentModel.DataAnnotations;

namespace PortalProfesores.Domain.Entities;

/// <summary>
/// Representa una categoría de clases.
/// </summary>
public class Categoria
{
    [Key]
    public long IdCategoria { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Descripcion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public long? UsuarioActualizacion { get; set; }


    // Relaciones

    public ICollection<Clase> Clases { get; set; }
        = new List<Clase>();
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalProfesores.Domain.Entities;

/// <summary>
/// Representa una clase disponible en el portal.
/// </summary>
public class Clase
{
    [Key]
    public long IdClase { get; set; }

    public long IdIdioma { get; set; }

    public long IdNivel { get; set; }

    public long IdCategoria { get; set; }

    public long IdEstadoClase { get; set; }

    [Required]
    [MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    [MaxLength(500)]
    public string? UrlVideo { get; set; }

    public int? DuracionMinutos { get; set; }

    public int Orden { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public long? UsuarioActualizacion { get; set; }


    // Relaciones

    [ForeignKey(nameof(IdIdioma))]
    public Idioma? Idioma { get; set; }

    [ForeignKey(nameof(IdNivel))]
    public Nivel? Nivel { get; set; }

    [ForeignKey(nameof(IdCategoria))]
    public Categoria? Categoria { get; set; }

    [ForeignKey(nameof(IdEstadoClase))]
    public EstadoClase? EstadoClase { get; set; }

    public ICollection<Sesion> Sesiones { get; set; }
    = new List<Sesion>();
}
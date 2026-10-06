namespace PortalProfesores.Domain.DTOs.Clases;

/// <summary>
/// Información completa de una clase.
/// </summary>
public class ClaseDetalleDto
{
    public long IdClase { get; set; }

    public long IdIdioma { get; set; }

    public string Idioma { get; set; } = string.Empty;

    public long IdNivel { get; set; }

    public string Nivel { get; set; } = string.Empty;

    public long IdCategoria { get; set; }

    public string Categoria { get; set; } = string.Empty;

    public long IdEstadoClase { get; set; }

    public string EstadoClase { get; set; } = string.Empty;

    public string Titulo { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public string? UrlVideo { get; set; }

    public int? DuracionMinutos { get; set; }

    public int Orden { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }
}
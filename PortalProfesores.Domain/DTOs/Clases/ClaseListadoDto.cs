namespace PortalProfesores.Domain.DTOs.Clases;

/// <summary>
/// Información resumida de una clase.
/// </summary>
public class ClaseListadoDto
{
    public long IdClase { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public string Idioma { get; set; } = string.Empty;

    public string Nivel { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public long IdEstadoClase { get; set; }

    public string EstadoClase { get; set; } = string.Empty;

    public int? DuracionMinutos { get; set; }
    public decimal Precio { get; set; }

    public long IdMoneda { get; set; }

    public string Moneda { get; set; } = string.Empty;

    public string CodigoMoneda { get; set; } = string.Empty;

    public int Orden { get; set; }

}
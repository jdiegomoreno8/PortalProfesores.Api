namespace PortalProfesores.Domain.DTOs.Clases;

/// <summary>
/// Información necesaria para crear una clase.
/// </summary>
public class CrearClaseRequestDto
{
    public long IdIdioma { get; set; }

    public long IdNivel { get; set; }

    public long IdCategoria { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public string? UrlVideo { get; set; }

    public int? DuracionMinutos { get; set; }

    public int Orden { get; set; }
}
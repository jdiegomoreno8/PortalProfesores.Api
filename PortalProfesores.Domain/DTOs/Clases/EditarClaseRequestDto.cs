
namespace PortalProfesores.Domain.DTOs.Clases;

/// <summary>
/// Representa la información editable de una clase.
/// </summary>
public class EditarClaseRequestDto
{
    public long IdIdioma { get; set; }

    public long IdNivel { get; set; }

    public long IdCategoria { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public string? UrlVideo { get; set; }

    /// <summary>
    /// Precio actualizado de la clase en la moneda seleccionada.
    /// </summary>
    public decimal Precio { get; set; }

    /// <summary>
    /// Identificador de la moneda del catálogo Moneda.
    /// </summary>
    public long IdMoneda { get; set; }
}

namespace PortalProfesores.Domain.DTOs.Catalogos;

/// <summary>
/// Representa un elemento básico de un catálogo.
/// </summary>
public class CatalogoItemDto
{
    public long Id { get; set; }

    public string Nombre { get; set; } = string.Empty;
}
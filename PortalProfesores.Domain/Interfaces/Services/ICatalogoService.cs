using PortalProfesores.Domain.DTOs.Catalogos;

namespace PortalProfesores.Domain.Interfaces.Services;

/// <summary>
/// Define las operaciones relacionadas
/// con los catálogos generales del portal.
/// </summary>
public interface ICatalogoService
{
    /// <summary>
    /// Obtiene los tipos de documento disponibles.
    /// </summary>
    Task<IEnumerable<CatalogoItemDto>> ObtenerTiposDocumentoAsync();

    /// <summary>
    /// Obtiene las ciudades disponibles.
    /// </summary>
    Task<IEnumerable<CatalogoItemDto>> ObtenerCiudadesAsync();
}
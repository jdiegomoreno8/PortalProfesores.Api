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

    /// <summary>
    /// Obtiene los niveles académicos disponibles.
    /// </summary>
    Task<IEnumerable<CatalogoItemDto>> ObtenerNivelesAsync();

    /// <summary>
    /// Obtiene los niveles académicos disponibles.
    /// </summary>
    Task<IEnumerable<CatalogoItemDto>> ObtenerCategoriasAsync();

    /// <summary>
    /// Obtiene los niveles académicos disponibles.
    /// </summary>
    Task<IEnumerable<CatalogoItemDto>> ObtenerIdiomasAsync();

    /// <summary>
    /// Obtiene las monedas disponibles para las tarifas de las clases.
    /// </summary>
    Task<IEnumerable<CatalogoItemDto>> ObtenerMonedasAsync();
}
using PortalProfesores.Domain.DTOs.Catalogos;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Domain.Interfaces.Services;

namespace PortalProfesores.Infrastructure.Services.Catalogos;

/// <summary>
/// Implementación del servicio encargado
/// de consultar los catálogos generales del portal.
/// </summary>
public class CatalogoService : ICatalogoService
{
    private readonly ITipoDocumentoRepository _tipoDocumentoRepository;
    private readonly ICiudadRepository _ciudadRepository;

    public CatalogoService(
        ITipoDocumentoRepository tipoDocumentoRepository,
        ICiudadRepository ciudadRepository)
    {
        _tipoDocumentoRepository = tipoDocumentoRepository;
        _ciudadRepository = ciudadRepository;
    }

    /// <summary>
    /// Obtiene los tipos de documento disponibles.
    /// </summary>
    public async Task<IEnumerable<CatalogoItemDto>> ObtenerTiposDocumentoAsync()
    {
        var tiposDocumento =
            await _tipoDocumentoRepository.ObtenerTodosAsync();

        return tiposDocumento.Select(x => new CatalogoItemDto
        {
            Id = x.IdTipoDocumento,
            Nombre = x.Nombre
        });
    }

    /// <summary>
    /// Obtiene las ciudades disponibles.
    /// </summary>
    public async Task<IEnumerable<CatalogoItemDto>> ObtenerCiudadesAsync()
    {
        var ciudades =
            await _ciudadRepository.ObtenerTodosAsync();

        return ciudades.Select(x => new CatalogoItemDto
        {
            Id = x.IdCiudad,
            Nombre = x.Nombre
        });
    }
}
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
    private readonly INivelRepository _nivelRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IIdiomaRepository _idiomaRepository;
    private readonly IMonedaRepository _monedaRepository;

    public CatalogoService(
        ITipoDocumentoRepository tipoDocumentoRepository,
        ICiudadRepository ciudadRepository,
        INivelRepository nivelRepository,
        ICategoriaRepository categoriaRepository,
        IIdiomaRepository idiomaRepository,
        IMonedaRepository monedaRepository)

    {
        _tipoDocumentoRepository = tipoDocumentoRepository;
        _ciudadRepository = ciudadRepository;
        _nivelRepository = nivelRepository;
        _categoriaRepository = categoriaRepository;
        _idiomaRepository = idiomaRepository;
        _monedaRepository = monedaRepository;
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

    /// <summary>
    /// Obtiene los niveles académicos disponibles.
    /// </summary>
    public async Task<IEnumerable<CatalogoItemDto>> ObtenerNivelesAsync()
    {
        var niveles =
            await _nivelRepository.ObtenerTodosAsync();

        return niveles.Select(x => new CatalogoItemDto
        {
            Id = x.IdNivel,
            Nombre = x.Nombre
        });
    }

    //
    //    
    /// <summary>
    /// Obtiene los idiomas académicos disponibles.
    /// </summary>
    public async Task<IEnumerable<CatalogoItemDto>> ObtenerIdiomasAsync()
    {
        var niveles =
            await _idiomaRepository.ObtenerTodosAsync();

        return niveles.Select(x => new CatalogoItemDto
        {
            Id = x.IdIdioma,
            Nombre = x.Nombre
        });
    }

    /// <summary>
    /// Obtiene las categorias académicos disponibles.
    /// </summary>
    public async Task<IEnumerable<CatalogoItemDto>> ObtenerCategoriasAsync()
    {
        var niveles =
            await _categoriaRepository.ObtenerTodosAsync();

        return niveles.Select(x => new CatalogoItemDto
        {
            Id = x.IdCategoria,
            Nombre = x.Nombre
        });
    }
    /// <summary>
    /// Obtiene las monedas disponibles para las tarifas de las clases.
    /// </summary>
    public async Task<IEnumerable<CatalogoItemDto>> ObtenerMonedasAsync()
    {
        var monedas = await _monedaRepository.ObtenerTodosAsync();

        return monedas.Select(x => new CatalogoItemDto
        {
            Id = x.IdMoneda,
            Nombre = x.Nombre
        });
    }

}
using Microsoft.AspNetCore.Mvc;
using PortalProfesores.Domain.Interfaces.Services;

namespace PortalProfesores.Api.Controllers;

/// <summary>
/// Expone los catálogos generales utilizados
/// por las aplicaciones del portal.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CatalogosController : ControllerBase
{
    private readonly ICatalogoService _catalogoService;

    public CatalogosController(ICatalogoService catalogoService)
    {
        _catalogoService = catalogoService;
    }

    /// <summary>
    /// Obtiene los tipos de documento disponibles.
    /// </summary>
    [HttpGet("tipos-documento")]
    public async Task<IActionResult> ObtenerTiposDocumento()
    {
        var tiposDocumento =
            await _catalogoService.ObtenerTiposDocumentoAsync();

        return Ok(tiposDocumento);
    }

    /// <summary>
    /// Obtiene las ciudades disponibles.
    /// </summary>
    [HttpGet("ciudades")]
    public async Task<IActionResult> ObtenerCiudades()
    {
        var ciudades =
            await _catalogoService.ObtenerCiudadesAsync();

        return Ok(ciudades);
    }

    /// <summary>
    /// Obtiene los niveles académicos disponibles.
    /// </summary>
    [HttpGet("niveles")]
    public async Task<IActionResult> ObtenerNiveles()
    {
        var niveles =
            await _catalogoService.ObtenerNivelesAsync();

        return Ok(niveles);
    }

    /// <summary>
    /// Obtiene las categorias académicos disponibles.
    /// </summary>
    [HttpGet("categorias")]
    public async Task<IActionResult> ObtenerCategorias()
    {
        var categorias =
            await _catalogoService.ObtenerCategoriasAsync();

        return Ok(categorias);
    }

    /// <summary>
    /// Obtiene los idiomas académicos disponibles.
    /// </summary>
    [HttpGet("idiomas")]
    public async Task<IActionResult> ObtenerIdiomas()
    {
        var idiomas =
            await _catalogoService.ObtenerIdiomasAsync();

        return Ok(idiomas);
    }

    /// <summary>
    /// Obtiene las monedas disponibles para las tarifas de las clases.
    /// </summary>
    [HttpGet("monedas")]
    public async Task<IActionResult> ObtenerMonedas()
    {
        var monedas =
            await _catalogoService.ObtenerMonedasAsync();

        return Ok(monedas);
    }

}
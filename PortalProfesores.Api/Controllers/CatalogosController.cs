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
}
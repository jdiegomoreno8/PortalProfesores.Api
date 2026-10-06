using Microsoft.AspNetCore.Mvc;
using PortalProfesores.Domain.DTOs.Clases;
using PortalProfesores.Domain.Interfaces.Services;

namespace PortalProfesores.Api.Controllers;

/// <summary>
/// Expone las operaciones de consulta de clases.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClasesController : ControllerBase
{
    private readonly IClaseService _claseService;

    public ClasesController(
        IClaseService claseService)
    {
        _claseService = claseService;
    }

    /// <summary>
    /// Obtiene el listado de clases.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ObtenerListado()
    {
        var clases =
            await _claseService.ObtenerListadoAsync();

        return Ok(clases);
    }

    /// <summary>
    /// Obtiene una clase por su identificador.
    /// </summary>
    [HttpGet("{idClase:long}")]
    public async Task<IActionResult> ObtenerPorId(
        long idClase)
    {
        var clase =
            await _claseService.ObtenerPorIdAsync(idClase);

        if (clase is null)
        {
            return NotFound(new
            {
                Mensaje = "La clase no fue encontrada."
            });
        }

        return Ok(clase);
    }
    /// <summary>
    /// Crea una nueva clase.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CrearClase(
        CrearClaseRequestDto request)
    {
        await _claseService.CrearAsync(request);

        return Ok(new
        {
            Mensaje = "Clase creada correctamente."
        });
    }

    /// <summary>
    /// Actualiza una clase existente.
    /// </summary>
    [HttpPut("{idClase:long}")]
    public async Task<IActionResult> EditarClase(
        long idClase,
        EditarClaseRequestDto request)
    {
        await _claseService.EditarAsync(
            idClase,
            request);

        return Ok(new
        {
            Mensaje = "Clase actualizada correctamente."
        });
    }
    /// <summary>
    /// Suspende una clase para nuevas asignaciones.
    /// Las sesiones ya existentes no se eliminan.
    /// </summary>
    [HttpPut("{idClase:long}/suspender")]
    public async Task<IActionResult> SuspenderClase(
        long idClase)
    {
        await _claseService.SuspenderAsync(idClase);

        return Ok(new
        {
            Mensaje = "Clase suspendida correctamente."
        });
    }

    /// <summary>
    /// Activa nuevamente una clase para nuevas asignaciones.
    /// </summary>
    [HttpPut("{idClase:long}/activar")]
    public async Task<IActionResult> ActivarClase(
        long idClase)
    {
        await _claseService.ActivarAsync(idClase);

        return Ok(new
        {
            Mensaje = "Clase activada correctamente."
        });
    }
}
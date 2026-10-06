using Microsoft.EntityFrameworkCore;
using PortalProfesores.Domain.DTOs.Clases;
using PortalProfesores.Domain.Entities;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Infrastructure.Data;

namespace PortalProfesores.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de clases.
/// </summary>
public class ClaseRepository : IClaseRepository
{
    private readonly PortalProfesoresDbContext _context;

    public ClaseRepository(PortalProfesoresDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene todas las clases disponibles.
    /// </summary>
    public async Task<IEnumerable<Clase>> ObtenerListadoAsync()
    {
        return await _context.Clases
            .AsNoTracking()
            .Include(x => x.Idioma)
            .Include(x => x.Nivel)
            .Include(x => x.Categoria)
            .Include(x => x.EstadoClase)
            .OrderBy(x => x.Orden)
            .ThenBy(x => x.Titulo)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene una clase por su identificador.
    /// </summary>
    public async Task<Clase?> ObtenerPorIdAsync(long idClase)
    {
        return await _context.Clases
            .AsNoTracking()
            .Include(x => x.Idioma)
            .Include(x => x.Nivel)
            .Include(x => x.Categoria)
            .Include(x => x.EstadoClase)
            .FirstOrDefaultAsync(x => x.IdClase == idClase);
    }

    /// <summary>
    /// Crea una nueva clase.
    /// </summary>
    public async Task<Clase> CrearAsync(Clase clase)
    {
        await _context.Clases.AddAsync(clase);

        await _context.SaveChangesAsync();

        return clase;
    }

    /// <summary>
    /// Suspende una clase existente.
    /// </summary>
    public async Task<bool> SuspenderAsync(long idClase)
    {
        var clase = await _context.Clases
            .FirstOrDefaultAsync(x => x.IdClase == idClase);

        if (clase is null)
        {
            return false;
        }

        clase.IdEstadoClase = 2;

        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Activa nuevamente una clase suspendida.
    /// </summary>
    public async Task<bool> ActivarAsync(long idClase)
    {
        var clase = await _context.Clases
            .FirstOrDefaultAsync(x => x.IdClase == idClase);

        if (clase is null)
        {
            return false;
        }

        clase.IdEstadoClase = 1;

        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Actualiza una clase existente.
    /// </summary>
    public async Task<bool> EditarAsync(
        long idClase,
        EditarClaseRequestDto request)
    {
        var clase = await _context.Clases
            .FirstOrDefaultAsync(x => x.IdClase == idClase);

        if (clase is null)
        {
            return false;
        }

        clase.IdIdioma = request.IdIdioma;
        clase.IdNivel = request.IdNivel;
        clase.IdCategoria = request.IdCategoria;

        clase.Titulo = request.Titulo;
        clase.Descripcion = request.Descripcion;
        clase.UrlVideo = request.UrlVideo;

        clase.DuracionMinutos = request.DuracionMinutos;
        clase.Orden = request.Orden;

        clase.FechaActualizacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }
}
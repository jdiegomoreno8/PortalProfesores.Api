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
            .Include(x => x.Moneda)
            .OrderBy(x => x.IdIdioma)
            .ThenBy(x => x.IdNivel)
            .ThenBy(x => x.IdCategoria)
            .ThenBy(x => x.Orden)
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
            .Include(x => x.Moneda)
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
        EditarClaseRequestDto request,
         int duracionMinutos)
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
        clase.DuracionMinutos = duracionMinutos;
        clase.Precio = request.Precio;
        clase.IdMoneda = request.IdMoneda;
        clase.FechaActualizacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<int> ObtenerSiguienteOrdenAsync(
    long idIdioma,
    long idNivel,
    long idCategoria)
    {
        var ultimoOrden = await _context.Clases
            .Where(x =>
                x.IdIdioma == idIdioma &&
                x.IdNivel == idNivel &&
                x.IdCategoria == idCategoria)
            .Select(x => (int?)x.Orden)
            .MaxAsync();

        return (ultimoOrden ?? 0) + 1;
    }


    public async Task<bool> ProfesorPuedeImpartirClaseAsync(
    long idProfesor,
    long idClase)
    {
        return await _context.ProfesoresClases
            .AsNoTracking()
            .AnyAsync(x =>
                x.IdProfesor == idProfesor &&
                x.IdClase == idClase);
    }
}
using Microsoft.EntityFrameworkCore;
using PortalProfesores.Domain.Entities;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Infrastructure.Data;

namespace PortalProfesores.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de ciudades.
/// </summary>
public class CiudadRepository : ICiudadRepository
{
    private readonly PortalProfesoresDbContext _context;

    public CiudadRepository(PortalProfesoresDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene todas las ciudades disponibles.
    /// </summary>
    public async Task<IEnumerable<Ciudad>> ObtenerTodosAsync()
    {
        return await _context.Ciudades
            .AsNoTracking()
            .OrderBy(x => x.Nombre)
            .ToListAsync();
    }


    /// <summary>
    /// Obtiene una ciudad por su identificador.
    /// </summary>
    public async Task<Ciudad?> ObtenerPorIdAsync(long idCiudad)
    {
        return await _context.Ciudades
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdCiudad == idCiudad);
    }

}
using Microsoft.EntityFrameworkCore;
using PortalProfesores.Domain.Entities;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Infrastructure.Data;

namespace PortalProfesores.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de idiomas.
/// </summary>
public class IdiomaRepository : IIdiomaRepository
{
    private readonly PortalProfesoresDbContext _context;

    public IdiomaRepository(PortalProfesoresDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene todos los idiomas disponibles.
    /// </summary>
    public async Task<IEnumerable<Idioma>> ObtenerTodosAsync()
    {
        return await _context.Idiomas
            .AsNoTracking()
            .OrderBy(x => x.Nombre)
            .ToListAsync();
    }
}
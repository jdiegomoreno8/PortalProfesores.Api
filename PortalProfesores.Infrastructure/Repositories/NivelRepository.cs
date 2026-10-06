using Microsoft.EntityFrameworkCore;
using PortalProfesores.Domain.Entities;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Infrastructure.Data;

namespace PortalProfesores.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de niveles académicos.
/// </summary>
public class NivelRepository : INivelRepository
{
    private readonly PortalProfesoresDbContext _context;

    public NivelRepository(PortalProfesoresDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene todos los niveles académicos disponibles.
    /// </summary>
    public async Task<IEnumerable<Nivel>> ObtenerTodosAsync()
    {
        return await _context.Niveles
            .AsNoTracking()
            .OrderBy(x => x.Nombre)
            .ToListAsync();
    }
}
using Microsoft.EntityFrameworkCore;
using PortalProfesores.Domain.Entities;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Infrastructure.Data;

namespace PortalProfesores.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de categorías.
/// </summary>
public class CategoriaRepository : ICategoriaRepository
{
    private readonly PortalProfesoresDbContext _context;

    public CategoriaRepository(PortalProfesoresDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene todas las categorías disponibles.
    /// </summary>
    public async Task<IEnumerable<Categoria>> ObtenerTodosAsync()
    {
        return await _context.Categorias
            .AsNoTracking()
            .OrderBy(x => x.Nombre)
            .ToListAsync();
    }
}

using Microsoft.EntityFrameworkCore;
using PortalProfesores.Domain.Entities;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Infrastructure.Data;

namespace PortalProfesores.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de monedas.
/// </summary>
public class MonedaRepository : IMonedaRepository
{
    private readonly PortalProfesoresDbContext _context;

    public MonedaRepository(PortalProfesoresDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene todas las monedas registradas.
    /// </summary>
    public async Task<IEnumerable<Moneda>> ObtenerTodosAsync()
    {
        return await _context.Monedas
            .AsNoTracking()
            .OrderBy(x => x.Nombre)
            .ToListAsync();
    }
}

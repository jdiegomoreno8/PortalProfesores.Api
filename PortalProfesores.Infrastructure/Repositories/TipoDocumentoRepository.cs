using Microsoft.EntityFrameworkCore;
using PortalProfesores.Domain.Entities;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Infrastructure.Data;

namespace PortalProfesores.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de tipos de documento.
/// </summary>
public class TipoDocumentoRepository : ITipoDocumentoRepository
{
    private readonly PortalProfesoresDbContext _context;

    public TipoDocumentoRepository(PortalProfesoresDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene todos los tipos de documento disponibles.
    /// </summary>
    public async Task<IEnumerable<TipoDocumento>> ObtenerTodosAsync()
    {
        return await _context.TiposDocumento
            .AsNoTracking()
            .OrderBy(x => x.Nombre)
            .ToListAsync();
    }
}

using Microsoft.EntityFrameworkCore;
using PortalProfesores.Domain.Entities;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Infrastructure.Data;

namespace PortalProfesores.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de configuraciones.
/// </summary>
public class ConfiguracionRepository : IConfiguracionRepository
{
    private readonly PortalProfesoresDbContext _context;

    public ConfiguracionRepository(PortalProfesoresDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene una configuración mediante su clave.
    /// </summary>
    public async Task<Configuracion?> ObtenerPorClaveAsync(string clave)
    {
        return await _context.Configuraciones
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Clave == clave);
    }
}

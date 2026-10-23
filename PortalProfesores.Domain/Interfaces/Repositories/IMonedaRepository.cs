
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Domain.Interfaces.Repositories;

/// <summary>
/// Define las operaciones de acceso a datos
/// para las monedas.
/// </summary>
public interface IMonedaRepository
{
    /// <summary>
    /// Obtiene todas las monedas registradas.
    /// </summary>
    Task<IEnumerable<Moneda>> ObtenerTodosAsync();
}

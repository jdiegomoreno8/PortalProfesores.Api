using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Domain.Interfaces.Repositories;

/// <summary>
/// Define las operaciones de acceso a datos
/// para las ciudades.
/// </summary>
public interface ICiudadRepository
{
    /// <summary>
    /// Obtiene todas las ciudades disponibles.
    /// </summary>
    Task<IEnumerable<Ciudad>> ObtenerTodosAsync();
}
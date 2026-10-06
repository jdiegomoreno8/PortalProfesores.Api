using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Domain.Interfaces.Repositories;

/// <summary>
/// Define las operaciones de acceso a datos
/// para los niveles académicos.
/// </summary>
public interface INivelRepository
{
    /// <summary>
    /// Obtiene todos los niveles académicos disponibles.
    /// </summary>
    Task<IEnumerable<Nivel>> ObtenerTodosAsync();
}
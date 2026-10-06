using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Domain.Interfaces.Repositories;

/// <summary>
/// Define las operaciones de acceso a datos
/// para los idiomas.
/// </summary>
public interface IIdiomaRepository
{
    /// <summary>
    /// Obtiene todos los idiomas disponibles.
    /// </summary>
    Task<IEnumerable<Idioma>> ObtenerTodosAsync();
}
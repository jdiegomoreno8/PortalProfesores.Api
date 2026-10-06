using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Domain.Interfaces.Repositories;

/// <summary>
/// Define las operaciones de acceso a datos
/// para las categorías.
/// </summary>
public interface ICategoriaRepository
{
    /// <summary>
    /// Obtiene todas las categorías disponibles.
    /// </summary>
    Task<IEnumerable<Categoria>> ObtenerTodosAsync();
}
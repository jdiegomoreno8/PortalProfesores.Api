using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Domain.Interfaces.Repositories;

/// <summary>
/// Define las operaciones de acceso a datos
/// para los tipos de documento.
/// </summary>
public interface ITipoDocumentoRepository
{
    /// <summary>
    /// Obtiene todos los tipos de documento disponibles.
    /// </summary>
    Task<IEnumerable<TipoDocumento>> ObtenerTodosAsync();
}
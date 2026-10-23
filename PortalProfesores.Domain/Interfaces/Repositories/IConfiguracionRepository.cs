
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Domain.Interfaces.Repositories;

/// <summary>
/// Define las operaciones de acceso a datos
/// relacionadas con las configuraciones del portal.
/// </summary>
public interface IConfiguracionRepository
{
    /// <summary>
    /// Obtiene una configuración mediante su clave.
    /// </summary>
    Task<Configuracion?> ObtenerPorClaveAsync(string clave);
}

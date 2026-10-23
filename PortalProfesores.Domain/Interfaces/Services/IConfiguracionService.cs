
namespace PortalProfesores.Domain.Interfaces.Services;

/// <summary>
/// Define las operaciones de consulta y validación
/// de las configuraciones del portal.
/// </summary>
public interface IConfiguracionService
{
    /// <summary>
    /// Obtiene una configuración entera positiva por su clave.
    /// </summary>
    Task<int> ObtenerEnteroPositivoAsync(string clave);
}

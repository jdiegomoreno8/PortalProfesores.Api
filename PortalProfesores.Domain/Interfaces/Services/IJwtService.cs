namespace PortalProfesores.Domain.Interfaces.Services;

/// <summary>
/// Define las operaciones necesarias para generar
/// tokens JWT para los usuarios autenticados.
/// </summary>
public interface IJwtService
{
    string GenerarToken(
        long idUsuario,
        string correo,
        string nombreCompleto,
        IEnumerable<string> roles);
}
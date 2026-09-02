using PortalProfesores.Domain.Interfaces.Services;

namespace PortalProfesores.Infrastructure.Services;

/// <summary>
/// Servicio encargado de generar tokens JWT.
/// La implementación completa se realizará
/// en el siguiente bloque.
/// </summary>
public class JwtService : IJwtService
{
    public string GenerarToken(long idUsuario,
                               string correo,
                               string nombreCompleto)
    {
        throw new NotImplementedException();
    }
}
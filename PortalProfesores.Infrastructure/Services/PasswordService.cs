using BCrypt.Net;
using PortalProfesores.Domain.Interfaces.Services;

namespace PortalProfesores.Infrastructure.Services;

/// <summary>
/// Servicio encargado del cifrado y validación
/// de contraseñas utilizando BCrypt.
/// </summary>
public class PasswordService : IPasswordService
{
    /// <summary>
    /// Genera el hash de una contraseña.
    /// </summary>
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// Verifica una contraseña contra su hash.
    /// </summary>
    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
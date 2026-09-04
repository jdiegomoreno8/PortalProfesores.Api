using PortalProfesores.Domain.Interfaces.Services;

namespace PortalProfesores.Api.Utilities;

/// <summary>
/// Utilidad temporal para generar hashes BCrypt.
/// Solo se utilizará durante las pruebas iniciales.
/// </summary>
public static class PasswordGenerator
{
    public static string Generate(IPasswordService passwordService)
    {
        return passwordService.HashPassword("123456");
    }
}
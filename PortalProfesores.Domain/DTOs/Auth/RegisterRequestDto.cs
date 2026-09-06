namespace PortalProfesores.Domain.DTOs.Auth;

/// <summary>
/// Información requerida para registrar un usuario.
/// </summary>
public class RegisterRequestDto
{
    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string CorreoElectronico { get; set; } = string.Empty;

    public string Clave { get; set; } = string.Empty;
}
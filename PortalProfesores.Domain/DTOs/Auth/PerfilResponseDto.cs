namespace PortalProfesores.Domain.DTOs.Auth;

/// <summary>
/// Información del usuario autenticado.
/// </summary>
public class PerfilResponseDto
{
    public long IdUsuario { get; set; }

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string CorreoElectronico { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;
}
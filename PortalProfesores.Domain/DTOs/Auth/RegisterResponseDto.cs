namespace PortalProfesores.Domain.DTOs.Auth;

/// <summary>
/// Resultado del registro de un usuario.
/// </summary>
public class RegisterResponseDto
{
    public long IdUsuario { get; set; }

    public string Mensaje { get; set; } = string.Empty;
}
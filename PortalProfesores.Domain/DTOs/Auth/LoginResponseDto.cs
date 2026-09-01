namespace PortalProfesores.Domain.DTOs.Auth;

public class LoginResponseDto
{
    public long IdUsuario { get; set; }

    public string NombreCompleto { get; set; } = string.Empty;

    public string CorreoElectronico { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;
}
namespace PortalProfesores.Domain.DTOs.Auth;

public class LoginRequestDto
{
    public string CorreoElectronico { get; set; } = string.Empty;

    public string Clave { get; set; } = string.Empty;
}
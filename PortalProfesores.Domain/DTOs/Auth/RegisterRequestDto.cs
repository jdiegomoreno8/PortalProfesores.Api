namespace PortalProfesores.Domain.DTOs.Auth;

/// <summary>
/// Información requerida para registrar un estudiante.
/// El rol se asigna automáticamente en el backend.
/// </summary>
public class RegisterRequestDto
{
    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public long IdTipoDocumento { get; set; }

    public string NumeroDocumento { get; set; } = string.Empty;

    public string CorreoElectronico { get; set; } = string.Empty;

    public string Clave { get; set; } = string.Empty;
}
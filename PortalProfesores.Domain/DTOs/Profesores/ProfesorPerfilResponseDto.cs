namespace PortalProfesores.Domain.DTOs.Profesores;

/// <summary>
/// Información pública del perfil del profesor autenticado.
/// </summary>
public class ProfesorPerfilResponseDto
{
    public long IdUsuario { get; set; }

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string CorreoElectronico { get; set; } = string.Empty;

    public string? Telefono { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public long? IdCiudad { get; set; }

    public string? Ciudad { get; set; }

    public string? FotoPerfilUrl { get; set; }

    public string Estado { get; set; } = string.Empty;

    public DateTime FechaIngreso { get; set; }
}
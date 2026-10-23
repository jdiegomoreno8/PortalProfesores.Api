namespace PortalProfesores.Domain.DTOs.Profesores;

/// <summary>
/// Información pública de un profesor
/// utilizada en la Landing.
/// </summary>
public class ProfesorPublicoDto
{
    public long IdProfesor { get; set; }

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string? Ciudad { get; set; }

    public string? Presentacion { get; set; }

    public string? Experiencia { get; set; }

    public string? Biografia { get; set; }

    public string? FotoPerfilUrl { get; set; }
}
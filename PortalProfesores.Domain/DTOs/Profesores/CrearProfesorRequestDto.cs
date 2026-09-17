namespace PortalProfesores.Domain.DTOs.Profesores;

/// <summary>
/// Información requerida para que un administrador
/// registre un nuevo profesor.
/// </summary>
public class CrearProfesorRequestDto
{
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;

    public long IdTipoDocumento { get; set; }
    public string NumeroDocumento { get; set; } = string.Empty;

    public string CorreoElectronico { get; set; } = string.Empty;
    public string Clave { get; set; } = string.Empty;

    public string? Telefono { get; set; }
    public DateOnly? FechaNacimiento { get; set; }
    public long? IdCiudad { get; set; }

    public string? Presentacion { get; set; }
    public string? Experiencia { get; set; }
    public string? Biografia { get; set; }
}
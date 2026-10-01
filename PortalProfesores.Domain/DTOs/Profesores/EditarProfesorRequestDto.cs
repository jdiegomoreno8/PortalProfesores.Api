namespace PortalProfesores.Domain.DTOs.Profesores;

/// <summary>
/// Información editable de un profesor desde el módulo administrativo.
/// </summary>
public class EditarProfesorRequestDto
{
    // Datos personales
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;

    public long IdTipoDocumento { get; set; }
    public string NumeroDocumento { get; set; } = string.Empty;

    public string CorreoElectronico { get; set; } = string.Empty;

    public string? Telefono { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public long? IdCiudad { get; set; }

    // Información profesional
    public string? Presentacion { get; set; }
    public string? Experiencia { get; set; }
    public string? Biografia { get; set; }
}
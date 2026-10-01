namespace PortalProfesores.Domain.DTOs.Profesores;

/// <summary>
/// Información completa y segura de un profesor
/// para su consulta desde el módulo administrativo.
/// </summary>
public class ProfesorDetalleDto
{
    // Identificación
    public long IdProfesor { get; set; }
    public long IdPersona { get; set; }

    public long IdTipoDocumento { get; set; }
    public string TipoDocumento { get; set; } = string.Empty;
    public string NumeroDocumento { get; set; } = string.Empty;

    // Datos personales
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public DateOnly? FechaNacimiento { get; set; }
    public long? IdCiudad { get; set; }
    public string? Ciudad { get; set; }

    // Estado de la cuenta
    public string Estado { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;

    // Información profesional
    public string? Presentacion { get; set; }
    public string? Experiencia { get; set; }
    public string? Biografia { get; set; }

    // Fechas relevantes
    public DateTime FechaIngreso { get; set; }
}
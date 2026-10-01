namespace PortalProfesores.Domain.DTOs.Profesores;

/// <summary>
/// Información resumida de un profesor
/// utilizada en el listado administrativo.
/// </summary>
public class ProfesorListadoDto
{
    public long IdProfesor { get; set; }

    public long IdPersona { get; set; }

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string CorreoElectronico { get; set; } = string.Empty;

    public string? Telefono { get; set; }

    public long? IdCiudad { get; set; }

    public string? Ciudad { get; set; }

    public string Estado { get; set; } = string.Empty;

    public DateTime FechaIngreso { get; set; }
}
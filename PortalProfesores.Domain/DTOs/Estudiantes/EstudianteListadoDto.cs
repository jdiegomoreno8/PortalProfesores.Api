namespace PortalProfesores.Domain.DTOs.Estudiantes;

/// <summary>
/// Información resumida de un estudiante
/// utilizada para el listado administrativo.
///
/// Este DTO contiene únicamente los datos necesarios
/// para representar al estudiante en la tabla de gestión.
/// </summary>
public class EstudianteListadoDto
{
    /// <summary>
    /// Identificador único del estudiante.
    /// </summary>
    public long IdEstudiante { get; set; }

    /// <summary>
    /// Identificador de la persona asociada.
    /// </summary>
    public long IdPersona { get; set; }

    /// <summary>
    /// Nombres del estudiante.
    /// </summary>
    public string Nombres { get; set; } = string.Empty;

    /// <summary>
    /// Apellidos del estudiante.
    /// </summary>
    public string Apellidos { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del estudiante.
    /// </summary>
    public string CorreoElectronico { get; set; } = string.Empty;

    /// <summary>
    /// Teléfono del estudiante.
    /// </summary>
    public string? Telefono { get; set; }

    /// <summary>
    /// Ciudad de residencia del estudiante.
    /// </summary>
    public string? Ciudad { get; set; }

    /// <summary>
    /// Nivel académico actual del estudiante.
    /// </summary>
    public string? Nivel { get; set; }

    /// <summary>
    /// Estado actual de la cuenta del estudiante.
    /// </summary>
    public string Estado { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de ingreso del estudiante al portal.
    /// </summary>
    public DateTime FechaIngreso { get; set; }
}
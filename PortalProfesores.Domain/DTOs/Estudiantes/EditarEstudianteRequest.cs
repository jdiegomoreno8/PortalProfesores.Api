namespace PortalProfesores.Domain.DTOs.Estudiantes;

/// <summary>
/// Información necesaria para actualizar
/// los datos de un estudiante.
/// </summary>
public class EditarEstudianteRequest
{
    /// <summary>
    /// Nombres del estudiante.
    /// </summary>
    public string Nombres { get; set; } = string.Empty;

    /// <summary>
    /// Apellidos del estudiante.
    /// </summary>
    public string Apellidos { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de documento del estudiante.
    /// </summary>
    public long IdTipoDocumento { get; set; }

    /// <summary>
    /// Número de documento del estudiante.
    /// </summary>
    public string NumeroDocumento { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del estudiante.
    /// </summary>
    public string CorreoElectronico { get; set; } = string.Empty;

    /// <summary>
    /// Teléfono del estudiante.
    /// </summary>
    public string? Telefono { get; set; }

    /// <summary>
    /// Fecha de nacimiento del estudiante.
    /// </summary>
    public DateTime? FechaNacimiento { get; set; }

    /// <summary>
    /// Ciudad de residencia del estudiante.
    /// </summary>
    public long? IdCiudad { get; set; }

    /// <summary>
    /// Nivel académico del estudiante.
    /// </summary>
    public long? IdNivel { get; set; }

    /// <summary>
    /// Objetivo principal de aprendizaje.
    /// </summary>
    public string? ObjetivoAprendizaje { get; set; }

    /// <summary>
    /// Observaciones adicionales del estudiante.
    /// </summary>
    public string? Observaciones { get; set; }
}
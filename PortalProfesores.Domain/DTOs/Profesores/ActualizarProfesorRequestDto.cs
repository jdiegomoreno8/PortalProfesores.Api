namespace PortalProfesores.Domain.DTOs.Profesores;

/// <summary>
/// Información editable del perfil del profesor.
/// </summary>
public class ActualizarProfesorRequestDto
{
    /// <summary>
    /// Nombres.
    /// </summary>
    public string Nombres { get; set; } = string.Empty;

    /// <summary>
    /// Apellidos.
    /// </summary>
    public string Apellidos { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico.
    /// </summary>
    public string CorreoElectronico { get; set; } = string.Empty;

    /// <summary>
    /// Teléfono.
    /// </summary>
    public string? Telefono { get; set; }

    /// <summary>
    /// Fecha de nacimiento.
    /// </summary>
    public DateOnly? FechaNacimiento { get; set; }

    /// <summary>
    /// Ciudad.
    /// </summary>
    public long? IdCiudad { get; set; }
}
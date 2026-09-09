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
}
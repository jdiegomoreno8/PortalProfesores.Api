namespace PortalProfesores.Domain.DTOs.Profesores;

/// <summary>
/// Información pública del perfil del profesor autenticado.
/// </summary>
public class ProfesorPerfilResponseDto
{
    /// <summary>
    /// Identificador del usuario.
    /// </summary>
    public long IdUsuario { get; set; }

    /// <summary>
    /// Nombres del profesor.
    /// </summary>
    public string Nombres { get; set; } = string.Empty;

    /// <summary>
    /// Apellidos del profesor.
    /// </summary>
    public string Apellidos { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico.
    /// </summary>
    public string CorreoElectronico { get; set; } = string.Empty;

    /// <summary>
    /// Estado del usuario.
    /// </summary>
    public string Estado { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de ingreso al portal.
    /// </summary>
    public DateTime FechaIngreso { get; set; }
}
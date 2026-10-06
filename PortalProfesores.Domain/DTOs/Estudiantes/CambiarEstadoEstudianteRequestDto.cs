namespace PortalProfesores.Domain.DTOs.Estudiantes;

/// <summary>
/// Representa la solicitud para cambiar
/// el estado de un estudiante.
/// </summary>
public class CambiarEstadoEstudianteRequestDto
{
    /// <summary>
    /// Identificador del nuevo estado del usuario.
    /// </summary>
    public long IdEstadoUsuario { get; set; }
}
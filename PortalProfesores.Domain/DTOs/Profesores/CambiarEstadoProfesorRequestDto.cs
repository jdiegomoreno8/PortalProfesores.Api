namespace PortalProfesores.Domain.DTOs.Profesores;

/// <summary>
/// Representa la solicitud para cambiar el estado de un profesor.
/// </summary>
public class CambiarEstadoProfesorRequestDto
{
    public long IdEstadoUsuario { get; set; }
}
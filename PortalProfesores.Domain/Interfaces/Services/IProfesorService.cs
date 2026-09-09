using PortalProfesores.Domain.DTOs.Profesores;

namespace PortalProfesores.Domain.Interfaces.Services;

/// <summary>
/// Define las operaciones relacionadas con la administración
/// del perfil del profesor.
/// </summary>
public interface IProfesorService
{
    /// <summary>
    /// Obtiene la información del profesor autenticado.
    /// </summary>
    /// <param name="idUsuario">
    /// Identificador del usuario autenticado.
    /// </param>
    Task<ProfesorPerfilResponseDto> ObtenerPerfilAsync(
        long idUsuario);

    /// <summary>
    /// Actualiza la información del perfil del profesor.
    /// </summary>
    /// <param name="idUsuario">
    /// Identificador del usuario autenticado.
    /// </param>
    /// <param name="request">
    /// Información a actualizar.
    /// </param>
    Task ActualizarPerfilAsync(
        long idUsuario,
        ActualizarProfesorRequestDto request);
}
using PortalProfesores.Domain.DTOs.Estudiantes;

namespace PortalProfesores.Domain.Interfaces.Repositories;

/// <summary>
/// Define las operaciones de acceso a datos
/// relacionadas con los estudiantes.
/// </summary>
public interface IEstudianteRepository
{
    /// <summary>
    /// Obtiene el detalle de un estudiante a partir
    /// de su identificador.
    /// </summary>
    Task<EstudianteDetalleDto?> ObtenerPorIdAsync(
        long idEstudiante);

    /// <summary>
    /// Obtiene el listado resumido de estudiantes
    /// para la gestión administrativa.
    /// </summary>
    Task<IEnumerable<EstudianteListadoDto>>
        ObtenerListadoAsync();

    /// <summary>
    /// Actualiza la información personal y académica
    /// de un estudiante.
    /// </summary>
    /// <param name="idEstudiante">
    /// Identificador del estudiante.
    /// </param>
    /// <param name="request">
    /// Información que se desea actualizar.
    /// </param>
    /// <returns>
    /// true si el estudiante fue actualizado correctamente;
    /// false si no existe.
    /// </returns>
    Task<bool> EditarAsync(
        long idEstudiante,
        EditarEstudianteRequest request);


    /// <summary>
    /// Cambia el estado del usuario asociado a un estudiante.
    /// </summary>
    Task<bool> CambiarEstadoAsync(
        long idEstudiante,
        long idEstadoUsuario);
}
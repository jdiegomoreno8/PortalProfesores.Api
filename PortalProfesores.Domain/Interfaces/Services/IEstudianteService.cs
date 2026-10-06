using PortalProfesores.Domain.DTOs.Estudiantes;

namespace PortalProfesores.Domain.Interfaces.Services;

/// <summary>
/// Define las operaciones relacionadas
/// con la gestión de estudiantes.
/// </summary>
public interface IEstudianteService
{
    /// <summary>
    /// Obtiene el detalle completo de un estudiante
    /// a partir de su identificador.
    /// </summary>
    Task<EstudianteDetalleDto?> ObtenerDetalleEstudianteAsync(
        long idEstudiante);

    /// <summary>
    /// Obtiene el listado de estudiantes registrados
    /// para la gestión administrativa.
    /// </summary>
    Task<IEnumerable<EstudianteListadoDto>> ObtenerEstudiantesAsync();
    /// <summary>
    /// Actualiza la información personal y académica
    /// de un estudiante.
    /// </summary>
    /// <param name="idEstudiante">
    /// Identificador del estudiante que se desea actualizar.
    /// </param>
    /// <param name="request">
    /// Información actualizada del estudiante.
    /// </param>
    /// <returns>
    /// True si el estudiante fue actualizado correctamente;
    /// false si no existe.
    /// </returns>
    Task<bool> EditarEstudianteAsync(
        long idEstudiante,
        EditarEstudianteRequest request);
}
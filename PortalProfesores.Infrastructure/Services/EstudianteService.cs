using PortalProfesores.Domain.DTOs.Estudiantes;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Domain.Interfaces.Services;

namespace PortalProfesores.Infrastructure.Services;

/// <summary>
/// Servicio encargado de las operaciones relacionadas
/// con la gestión de estudiantes.
///
/// En esta primera etapa se implementa únicamente
/// la consulta del detalle de un estudiante.
/// </summary>
public class EstudianteService : IEstudianteService
{
    private readonly IEstudianteRepository _estudianteRepository;

    /// <summary>
    /// Inicializa una nueva instancia del servicio
    /// de estudiantes.
    /// </summary>
    /// <param name="estudianteRepository">
    /// Repositorio encargado del acceso a los datos
    /// del estudiante.
    /// </param>
    public EstudianteService(
        IEstudianteRepository estudianteRepository)
    {
        _estudianteRepository = estudianteRepository;
    }

    /// <summary>
    /// Obtiene el detalle completo de un estudiante.
    ///
    /// La consulta se delega al repositorio, que es
    /// responsable de obtener y proyectar la información
    /// de Persona, Usuario, Nivel y EstudianteDetalle
    /// hacia EstudianteDetalleDto.
    /// </summary>
    /// <param name="idEstudiante">
    /// Identificador del estudiante que se desea consultar.
    /// </param>
    /// <returns>
    /// El detalle del estudiante si existe;
    /// de lo contrario, null.
    /// </returns>
    public async Task<EstudianteDetalleDto?>
        ObtenerDetalleEstudianteAsync(long idEstudiante)
    {
        // Un identificador menor o igual a cero
        // no representa un estudiante válido.
        if (idEstudiante <= 0)
        {
            return null;
        }

        // Delegamos la consulta al repositorio.
        return await _estudianteRepository
            .ObtenerPorIdAsync(idEstudiante);
    }

    /// <summary>
    /// Obtiene el listado de estudiantes para la gestión administrativa.
    ///
    /// La consulta se delega al repositorio, que es responsable
    /// de obtener la información y proyectarla hacia
    /// EstudianteListadoDto.
    /// </summary>
    /// <returns>
    /// Colección de estudiantes registrados.
    /// </returns>
    public async Task<IEnumerable<EstudianteListadoDto>>
        ObtenerEstudiantesAsync()
    {
        // Delegamos la consulta al repositorio.
        // El repositorio se encarga del acceso a la base de datos.
        return await _estudianteRepository
            .ObtenerListadoAsync();
    }

    /// <summary>
    /// Actualiza la información personal y académica
    /// de un estudiante.
    ///
    /// La operación se delega al repositorio, que es
    /// responsable de realizar la actualización en la
    /// base de datos.
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
    public async Task<bool> EditarEstudianteAsync(
        long idEstudiante,
        EditarEstudianteRequest request)
    {
        // Un identificador menor o igual a cero
        // no representa un estudiante válido.
        if (idEstudiante <= 0)
        {
            return false;
        }

        // Delegamos la actualización al repositorio.
        return await _estudianteRepository
            .EditarAsync(idEstudiante, request);
    }
}
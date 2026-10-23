using PortalProfesores.Domain.DTOs.Profesores;
using PortalProfesores.Domain.DTOs.Estudiantes;

namespace PortalProfesores.Domain.Interfaces.Services;

/// <summary>
/// Define las operaciones administrativas relacionadas
/// con la gestión de profesores.
/// </summary>
public interface IAdministradorService
{
    /// <summary>
    /// Crea un nuevo profesor y sus entidades relacionadas.
    /// </summary>
    Task<long> CrearProfesorAsync(CrearProfesorRequestDto request);
    Task<IEnumerable<ProfesorListadoDto>> ObtenerProfesoresAsync();

    Task<ProfesorDetalleDto?> ObtenerDetalleProfesorAsync(long idProfesor);

    Task EditarProfesorAsync(
      long idProfesor,
      EditarProfesorRequestDto request);

    Task CambiarEstadoProfesorAsync(
    long idProfesor,
    CambiarEstadoProfesorRequestDto request);

    /// <summary>
    /// Cambia el estado de un estudiante.
    /// </summary>
    Task CambiarEstadoEstudianteAsync(
        long idEstudiante,
        CambiarEstadoEstudianteRequestDto request);

    /// <summary>
    /// Permite al administrador reemplazar la fotografía
    /// de perfil de un profesor.
    /// </summary>
    Task CambiarFotoPerfilProfesorAsync(
        long idProfesor,
        Stream archivo,
        string nombreArchivo,
        string contentType);

    Task EliminarFotoPerfilProfesorAsync(long idProfesor);


    /// <summary>
    /// Permite al administrador cambiar la fotografía
    /// de perfil de un estudiante.
    /// </summary>
    Task CambiarFotoPerfilEstudianteAsync(
        long idEstudiante,
        Stream archivo,
        string nombreArchivo,
        string contentType);

    /// <summary>
    /// Permite al administrador eliminar la fotografía
    /// de perfil de un estudiante.
    /// </summary>
    Task EliminarFotoPerfilEstudianteAsync(long idEstudiante);


}
using PortalProfesores.Domain.DTOs.Profesores;

namespace PortalProfesores.Domain.Interfaces.Repositories;

/// <summary>
/// Repositorio para las operaciones de consulta
/// relacionadas con los profesores.
/// </summary>
public interface IProfesorRepository
{
    /// <summary>
    /// Obtiene el listado de profesores registrados.
    /// </summary>
    Task<IEnumerable<ProfesorListadoDto>> ObtenerListadoAsync();

    Task<ProfesorDetalleDto?> ObtenerDetalleAsync(long idProfesor);

    /// <summary>
    /// Obtiene la información pública de los profesores
    /// para la Landing.
    /// </summary>
    Task<IEnumerable<ProfesorPublicoDto>> ObtenerProfesoresPublicosAsync();

}
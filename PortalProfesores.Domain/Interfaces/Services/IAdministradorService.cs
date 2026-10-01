using PortalProfesores.Domain.DTOs.Profesores;

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
    Task CrearProfesorAsync(CrearProfesorRequestDto request);
    Task<IEnumerable<ProfesorListadoDto>> ObtenerProfesoresAsync();

    Task<ProfesorDetalleDto?> ObtenerDetalleProfesorAsync(long idProfesor);

    Task EditarProfesorAsync(
      long idProfesor,
      EditarProfesorRequestDto request);

    Task CambiarEstadoProfesorAsync(
    long idProfesor,
    CambiarEstadoProfesorRequestDto request);
}
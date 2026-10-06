using PortalProfesores.Domain.DTOs.Clases;

namespace PortalProfesores.Domain.Interfaces.Services;

/// <summary>
/// Define las operaciones relacionadas
/// con la consulta y gestión de clases.
/// </summary>
public interface IClaseService
{
    /// <summary>
    /// Obtiene el listado de clases.
    /// </summary>
    Task<IEnumerable<ClaseListadoDto>> ObtenerListadoAsync();

    /// <summary>
    /// Obtiene el detalle de una clase.
    /// </summary>
    Task<ClaseDetalleDto?> ObtenerPorIdAsync(long idClase);
    /// <summary>
    /// Crea una nueva clase.
    /// </summary>
    Task CrearAsync(CrearClaseRequestDto request);

    /// <summary>
    /// Suspende una clase existente.
    /// </summary>
    Task SuspenderAsync(long idClase);

    /// <summary>
    /// Activa nuevamente una clase suspendida.
    /// </summary>
    Task ActivarAsync(long idClase);

    /// <summary>
    /// Actualiza una clase existente.
    /// </summary>
    Task EditarAsync(
        long idClase,
        EditarClaseRequestDto request);
}
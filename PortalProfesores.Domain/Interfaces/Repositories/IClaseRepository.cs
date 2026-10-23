using PortalProfesores.Domain.DTOs.Clases;
using PortalProfesores.Domain.Entities;


namespace PortalProfesores.Domain.Interfaces.Repositories;

/// <summary>
/// Define las operaciones de acceso a datos
/// relacionadas con las clases.
/// </summary>
public interface IClaseRepository
{
    /// <summary>
    /// Obtiene el listado de clases.
    /// </summary>
    Task<IEnumerable<Clase>> ObtenerListadoAsync();

    /// <summary>
    /// Obtiene una clase a partir de su identificador.
    /// </summary>
    Task<Clase?> ObtenerPorIdAsync(long idClase);
    /// <summary>
    /// Crea una nueva clase.
    /// </summary>
    Task<Clase> CrearAsync(Clase clase);
    /// <summary>
    /// Actualiza una clase existente.
    /// </summary>
    Task<bool> EditarAsync(
        long idClase,
        EditarClaseRequestDto request,
        int duracionMinutos);

    Task<int> ObtenerSiguienteOrdenAsync(
      long idIdioma,
      long idNivel,
      long idCategoria);
    /// <summary>
    /// Suspende una clase existente.
    /// </summary>
    Task<bool> SuspenderAsync(long idClase);

    /// <summary>
    /// Activa nuevamente una clase suspendida.
    /// </summary>
    Task<bool> ActivarAsync(long idClase);

    Task<bool> ProfesorPuedeImpartirClaseAsync(
    long idProfesor,
    long idClase);
}
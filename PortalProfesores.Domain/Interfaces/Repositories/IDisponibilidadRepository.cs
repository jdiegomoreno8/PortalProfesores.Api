
using PortalProfesores.Domain.DTOs.Reservas;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Domain.Interfaces.Repositories;

/// <summary>
/// Define las operaciones de acceso a datos
/// para las disponibilidades de los profesores.
/// </summary>
public interface IDisponibilidadRepository
{
    /// <summary>
    /// Obtiene una disponibilidad por su identificador.
    /// </summary>
    Task<Disponibilidad?> ObtenerPorIdAsync(long idDisponibilidad);

    /// <summary>
    /// Obtiene las disponibilidades que pueden reservarse.
    /// </summary>
    Task<IEnumerable<DisponibilidadDisponibleDto>> ObtenerDisponiblesAsync();
}

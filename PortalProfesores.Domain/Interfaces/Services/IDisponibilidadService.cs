using PortalProfesores.Domain.DTOs.Disponibilidades;

namespace PortalProfesores.Domain.Interfaces.Services;

/// <summary>
/// Define las operaciones relacionadas
/// con la consulta y gestión de disponibilidades.
/// </summary>
public interface IDisponibilidadService
{
    /// <summary>
    /// Obtiene una disponibilidad por su identificador.
    /// </summary>
    Task<DisponibilidadResponseDto?> ObtenerPorIdAsync(
        long idDisponibilidad);
}
using PortalProfesores.Domain.DTOs.Disponibilidades;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Domain.Interfaces.Services;

namespace PortalProfesores.Infrastructure.Services;

/// <summary>
/// Implementa la lógica de consulta de disponibilidades.
/// </summary>
public class DisponibilidadService : IDisponibilidadService
{
    private readonly IDisponibilidadRepository _disponibilidadRepository;

    public DisponibilidadService(
        IDisponibilidadRepository disponibilidadRepository)
    {
        _disponibilidadRepository = disponibilidadRepository;
    }

    /// <summary>
    /// Obtiene una disponibilidad por su identificador.
    /// </summary>
    public async Task<DisponibilidadResponseDto?> ObtenerPorIdAsync(
        long idDisponibilidad)
    {
        if (idDisponibilidad <= 0)
        {
            throw new ArgumentException(
                "El identificador de la disponibilidad debe ser mayor que cero.",
                nameof(idDisponibilidad));
        }

        var disponibilidad = await _disponibilidadRepository
            .ObtenerPorIdAsync(idDisponibilidad);

        if (disponibilidad is null)
        {
            return null;
        }

        return new DisponibilidadResponseDto
        {
            IdDisponibilidad = disponibilidad.IdDisponibilidad,
            IdProfesor = disponibilidad.IdProfesor,
            EstadoDisponibilidad =
                disponibilidad.EstadoDisponibilidad?.Nombre
                ?? string.Empty,
            Fecha = disponibilidad.Fecha,
            HoraInicio = disponibilidad.HoraInicio,
            HoraFin = disponibilidad.HoraFin,
            IdZonaHoraria = disponibilidad.IdZonaHoraria,
            Observacion = disponibilidad.Observacion
        };
    }
}
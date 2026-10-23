
using Microsoft.EntityFrameworkCore;
using PortalProfesores.Domain.DTOs.Reservas;
using PortalProfesores.Domain.Entities;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Infrastructure.Data;

namespace PortalProfesores.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de disponibilidades
/// de los profesores.
/// </summary>
public class DisponibilidadRepository : IDisponibilidadRepository
{
    private readonly PortalProfesoresDbContext _context;

    public DisponibilidadRepository(PortalProfesoresDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene una disponibilidad por su identificador.
    /// </summary>


    public async Task<Disponibilidad?> ObtenerPorIdAsync(
 long idDisponibilidad)
    {
        return await _context.Disponibilidades
        .Include(d => d.EstadoDisponibilidad)
        .Include(d => d.ZonaHoraria)
        .FirstOrDefaultAsync(
        d => d.IdDisponibilidad == idDisponibilidad);
    }

    /// <summary>
    /// Obtiene las disponibilidades que pueden reservarse.
    /// </summary>
    public async Task<IEnumerable<DisponibilidadDisponibleDto>> ObtenerDisponiblesAsync()
    {
        return await _context.Disponibilidades
            .AsNoTracking()
            .Where(d => d.EstadoDisponibilidad != null
                && d.EstadoDisponibilidad.Nombre == "Disponible"
                 && d.Fecha.Date >= DateTime.Today)
            .OrderBy(d => d.Fecha)
            .ThenBy(d => d.HoraInicio)
            .Select(d => new DisponibilidadDisponibleDto
            {
                IdDisponibilidad = d.IdDisponibilidad,
                Fecha = d.Fecha,
                HoraInicio = d.HoraInicio,
                HoraFin = d.HoraFin,
                NombreProfesor = d.Profesor != null
                    && d.Profesor.Persona != null
                        ? (d.Profesor.Persona.Nombres + " "
                            + d.Profesor.Persona.Apellidos).Trim()
                        : string.Empty
            })
            .ToListAsync();
    }

}

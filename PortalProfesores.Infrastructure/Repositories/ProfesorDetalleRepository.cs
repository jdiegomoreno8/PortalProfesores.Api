using Microsoft.EntityFrameworkCore;
using PortalProfesores.Domain.Entities;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Infrastructure.Data;

namespace PortalProfesores.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de ProfesorDetalle utilizando
/// Entity Framework Core.
/// </summary>
public class ProfesorDetalleRepository : IProfesorDetalleRepository
{
    private readonly PortalProfesoresDbContext _context;

    public ProfesorDetalleRepository(PortalProfesoresDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene el detalle de un profesor a partir de la persona asociada.
    /// </summary>
    public async Task<ProfesorDetalle?> ObtenerPorPersonaAsync(long idPersona)
    {
        return await _context.ProfesoresDetalle
            .Include(x => x.Persona)
            .FirstOrDefaultAsync(x => x.IdPersona == idPersona);
    }

    public async Task<ProfesorDetalle?> ObtenerPorIdAsync(long idProfesor)
    {
        return await _context.ProfesoresDetalle
            .Include(x => x.Persona)
                .ThenInclude(persona => persona.Usuario)
            .FirstOrDefaultAsync(x => x.IdProfesor == idProfesor);
    }

    /// <summary>
    /// Registra un nuevo detalle de profesor.
    /// </summary>
    public Task<ProfesorDetalle> CrearAsync(
        ProfesorDetalle profesorDetalle)
    {
        _context.ProfesoresDetalle.Add(profesorDetalle);

        return Task.FromResult(profesorDetalle);
    }

    /// <summary>
    /// Actualiza el detalle de un profesor existente.
    /// </summary>
    public async Task ActualizarAsync(
        ProfesorDetalle profesorDetalle)
    {
        _context.ProfesoresDetalle.Update(profesorDetalle);

        await _context.SaveChangesAsync();
    }
}
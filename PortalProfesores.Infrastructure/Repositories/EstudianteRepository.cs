using Microsoft.EntityFrameworkCore;
using PortalProfesores.Domain.Entities;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Infrastructure.Data;

namespace PortalProfesores.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de estudiantes
/// utilizando Entity Framework Core.
/// </summary>
public class EstudianteRepository : IEstudianteRepository
{
    private readonly PortalProfesoresDbContext _context;

    public EstudianteRepository(
        PortalProfesoresDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene un estudiante a partir de su identificador.
    ///
    /// Se incluyen:
    /// - Persona: información personal del estudiante.
    /// - Nivel: nivel académico asociado.
    ///
    /// AsNoTracking se utiliza porque en esta operación
    /// solamente necesitamos consultar la información.
    /// </summary>
    public async Task<EstudianteDetalle?> ObtenerPorIdAsync(
        long idEstudiante)
    {
        return await _context.EstudiantesDetalle
            .AsNoTracking()
            .Include(estudiante => estudiante.Persona)
            .Include(estudiante => estudiante.Nivel)
            .FirstOrDefaultAsync(
                estudiante =>
                    estudiante.IdEstudiante == idEstudiante);
    }
}
using PortalProfesores.Domain.Entities;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Infrastructure.Data;

namespace PortalProfesores.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de personas.
/// </summary>
public class PersonaRepository : IPersonaRepository
{
    private readonly PortalProfesoresDbContext _context;

    public PersonaRepository(PortalProfesoresDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Registra una nueva persona.
    /// </summary>
    public Task<Persona> CrearAsync(Persona persona)
    {
        _context.Personas.Add(persona);

        return Task.FromResult(persona);
    }
}
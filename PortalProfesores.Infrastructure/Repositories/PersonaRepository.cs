using Microsoft.EntityFrameworkCore;
using PortalProfesores.Domain.Entities;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Infrastructure.Data;

namespace PortalProfesores.Infrastructure.Repositories;

public class PersonaRepository : IPersonaRepository
{
    private readonly PortalProfesoresDbContext _context;

    public PersonaRepository(PortalProfesoresDbContext context)
    {
        _context = context;
    }

    public Task<Persona> CrearAsync(Persona persona)
    {
        _context.Personas.Add(persona);
        return Task.FromResult(persona);
    }

    public async Task<bool> ExisteDocumentoAsync(
        string numeroDocumento,
        long idPersona)
    {
        return await _context.Personas
            .AnyAsync(persona =>
                persona.NumeroDocumento == numeroDocumento
                &&
                persona.IdPersona != idPersona);
    }
}
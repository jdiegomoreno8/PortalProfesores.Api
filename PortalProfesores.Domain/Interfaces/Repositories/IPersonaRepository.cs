using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Domain.Interfaces.Repositories;

/// <summary>
/// Define las operaciones de acceso a datos para las personas.
/// </summary>
public interface IPersonaRepository
{
    /// <summary>
    /// Registra una nueva persona.
    /// </summary>
    Task<Persona> CrearAsync(Persona persona);
}
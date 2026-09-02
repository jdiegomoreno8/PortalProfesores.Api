using Microsoft.EntityFrameworkCore;
using PortalProfesores.Domain.Entities;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Infrastructure.Data;

namespace PortalProfesores.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de usuarios utilizando Entity Framework Core.
/// </summary>
public class UsuarioRepository : IUsuarioRepository
{
    private readonly PortalProfesoresDbContext _context;

    public UsuarioRepository(PortalProfesoresDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Busca un usuario por su correo electrónico incluyendo
    /// la información de Persona y EstadoUsuario.
    /// </summary>
    public async Task<Usuario?> ObtenerPorCorreoAsync(string correoElectronico)
    {
        return await _context.Usuarios
            .Include(u => u.Persona)
            .Include(u => u.EstadoUsuario)
            .FirstOrDefaultAsync(u =>
                u.Persona.CorreoElectronico == correoElectronico);
    }
}
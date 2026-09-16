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
    /// Obtiene un usuario por su identificador.
    /// </summary>
    public async Task<Usuario?> ObtenerPorIdAsync(long idUsuario)
    {
        return await _context.Usuarios
            .Include(u => u.Persona)
            .Include(u => u.EstadoUsuario)
            .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);
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
         .Include(u => u.Roles)
             .ThenInclude(ur => ur.Rol)
         .FirstOrDefaultAsync(u =>
             u.Persona.CorreoElectronico == correoElectronico);
    }

    /// <summary>
    /// Actualiza la información de un usuario.
    /// </summary>
    public async Task ActualizarAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Verifica si el correo electrónico ya está registrado.
    /// </summary>
    public async Task<bool> ExisteCorreoAsync(string correoElectronico)
    {
        return await _context.Usuarios
            .Include(u => u.Persona)
            .AnyAsync(u => u.Persona.CorreoElectronico == correoElectronico);
    }
    /// <summary>
    /// Indica si existe otro usuario con el mismo correo electrónico.
    /// </summary>
    public async Task<bool> ExisteCorreoAsync(
    string correoElectronico,
    long idUsuario)
{
    return await _context.Usuarios
        .Include(u => u.Persona)
        .AnyAsync(u =>

            u.Persona.CorreoElectronico == correoElectronico
            &&
            u.IdUsuario != idUsuario);
}

    /// <summary>
    /// Registra un nuevo usuario en la base de datos.
    /// </summary>
    public Task<Usuario> CrearAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);

        return Task.FromResult(usuario);
    }
}
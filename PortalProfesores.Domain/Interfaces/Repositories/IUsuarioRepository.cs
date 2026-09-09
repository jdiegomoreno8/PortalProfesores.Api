using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Domain.Interfaces.Repositories;

/// <summary>
/// Define las operaciones de acceso a datos relacionadas con los usuarios.
/// </summary>
public interface IUsuarioRepository
{
    /// <summary>
    /// Obtiene un usuario por correo electrónico.
    /// </summary>
    /// 
    Task<Usuario?> ObtenerPorIdAsync(long idUsuario);
    Task<Usuario?> ObtenerPorCorreoAsync(string correoElectronico);
   
    /// <summary>
    /// Actualiza la información de un usuario.
    /// </summary>
    Task ActualizarAsync(Usuario usuario);

    /// <summary>
    /// Verifica si ya existe un usuario con el correo indicado.
    /// </summary>
    /// 
    Task<bool> ExisteCorreoAsync(
    string correoElectronico);

    Task<bool> ExisteCorreoAsync(string correoElectronico,
    long idUsuario);

    /// <summary>
    /// Registra un nuevo usuario.
    /// </summary>
    Task<Usuario> CrearAsync(Usuario usuario);
}
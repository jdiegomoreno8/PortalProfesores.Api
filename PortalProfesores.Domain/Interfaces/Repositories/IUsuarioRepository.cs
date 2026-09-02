using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Domain.Interfaces.Repositories;

/// <summary>
/// Define las operaciones de acceso a datos relacionadas con los usuarios.
/// </summary>
public interface IUsuarioRepository
{
    /// <summary>
    /// Obtiene un usuario a partir de su correo electrónico.
    /// </summary>
    /// <param name="correoElectronico">Correo registrado en la plataforma.</param>
    /// <returns>Usuario encontrado o null.</returns>
    Task<Usuario?> ObtenerPorCorreoAsync(string correoElectronico);
}
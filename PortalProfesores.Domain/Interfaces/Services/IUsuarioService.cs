using PortalProfesores.Domain.DTOs.Usuario;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Domain.Interfaces.Services;

/// <summary>
/// Define la lógica de negocio relacionada con los usuarios.
/// </summary>
public interface IUsuarioService
{
    /// <summary>
    /// Obtiene un usuario utilizando su correo electrónico.
    /// </summary>
    Task<Usuario?> ObtenerPorCorreoAsync(string correoElectronico);

    /// <summary>
    /// Obtiene el perfil del usuario autenticado.
    /// </summary>
    Task<UsuarioPerfilDto> ObtenerPerfilAsync(long idUsuario);
}
using PortalProfesores.Domain.Entities;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Domain.Interfaces.Services;

namespace PortalProfesores.Infrastructure.Services;

/// <summary>
/// Implementa la lógica de negocio relacionada con los usuarios.
/// </summary>
public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    /// <summary>
    /// Obtiene un usuario por su correo electrónico.
    /// </summary>
    /// <param name="correoElectronico">Correo electrónico del usuario.</param>
    /// <returns>Usuario encontrado o null.</returns>
    public async Task<Usuario?> ObtenerPorCorreoAsync(string correoElectronico)
    {
        if (string.IsNullOrWhiteSpace(correoElectronico))
            throw new ArgumentException("El correo electrónico es obligatorio.");

        return await _usuarioRepository.ObtenerPorCorreoAsync(correoElectronico.Trim());
    }
}
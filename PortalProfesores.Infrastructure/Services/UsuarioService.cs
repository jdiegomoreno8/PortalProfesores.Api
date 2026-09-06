using PortalProfesores.Domain.DTOs.Usuario;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Domain.Interfaces.Services;

namespace PortalProfesores.Infrastructure.Services;

/// <summary>
/// Lógica de negocio relacionada con usuarios.
/// </summary>
public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Domain.Entities.Usuario?> ObtenerPorCorreoAsync(string correoElectronico)
    {
        return await _usuarioRepository.ObtenerPorCorreoAsync(correoElectronico);
    }

    public async Task<UsuarioPerfilDto> ObtenerPerfilAsync(long idUsuario)
    {
        var usuario = await _usuarioRepository.ObtenerPorIdAsync(idUsuario);

        if (usuario == null)
            throw new Exception("Usuario no encontrado.");

        return new UsuarioPerfilDto
        {
            IdUsuario = usuario.IdUsuario,
            Nombres = usuario.Persona.Nombres,
            Apellidos = usuario.Persona.Apellidos,
            CorreoElectronico = usuario.Persona.CorreoElectronico,
            Estado = usuario.EstadoUsuario.Nombre
        };
    }
}
using PortalProfesores.Domain.DTOs.Auth;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Domain.Interfaces.Services;
namespace PortalProfesores.Infrastructure.Services;

/// <summary>
/// Servicio responsable del proceso de autenticación.
/// En este bloque únicamente valida la existencia y estado del usuario.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public AuthService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        // Validar información de entrada
        if (string.IsNullOrWhiteSpace(request.CorreoElectronico))
            throw new ArgumentException("Debe ingresar un correo electrónico.");

        // Buscar usuario
        var usuario =await _usuarioRepository.ObtenerPorCorreoAsync(request.CorreoElectronico);

        if (usuario == null)
            throw new Exception("El usuario no existe.");

        // Validar estado
        if (!usuario.EstadoUsuario.Nombre.Equals("Activo", StringComparison.OrdinalIgnoreCase))
            throw new Exception("El usuario no se encuentra activo.");

        // En este bloque aún no se valida contraseña ni se genera JWT.
        return new LoginResponseDto
        {
            IdUsuario = usuario.IdUsuario,
            NombreCompleto = $"{usuario.Persona.Nombres} {usuario.Persona.Apellidos}",
            CorreoElectronico = usuario.Persona.CorreoElectronico,
            Token = string.Empty
        };
    }
}
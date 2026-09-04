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
	private readonly IPasswordService _passwordService;
	private readonly IJwtService _jwtService;

    public AuthService(
        IUsuarioRepository usuarioRepository, 
        IPasswordService passwordService, 
        IJwtService jwtService)
    {
        _usuarioRepository = usuarioRepository;
		_passwordService = passwordService;
		_jwtService = jwtService;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        // Validar información de entrada
        if (string.IsNullOrWhiteSpace(request.CorreoElectronico))
            throw new ArgumentException("Debe ingresar un correo electrónico.");

        // Buscar usuario
        var usuario =await _usuarioRepository.ObtenerPorCorreoAsync(request.CorreoElectronico);

        if (usuario == null)
            throw new Exception("Correo electrónico o contraseña incorrectos.");

        // Validar estado
        if (!usuario.EstadoUsuario.Nombre.Equals("Activo", StringComparison.OrdinalIgnoreCase))
            throw new Exception("El usuario no se encuentra activo.");

        if (string.IsNullOrWhiteSpace(usuario.ClaveHash))
        {
            throw new Exception("La cuenta no tiene una contraseña configurada.");
        }

        if (!_passwordService.VerifyPassword(
        request.Clave,usuario.ClaveHash))
		{
			throw new Exception("Correo electrónico o contraseña incorrectos.");
		}
		//Último acceso del usuario
		usuario.UltimoAcceso = DateTime.UtcNow;

		await _usuarioRepository.ActualizarAsync(usuario);
		
		var token = _jwtService.GenerarToken(
			usuario.IdUsuario,
			usuario.Persona.CorreoElectronico,
			$"{usuario.Persona.Nombres} {usuario.Persona.Apellidos}");

      // // Retornar la información del usuario junto con el JWT generado.
        return new LoginResponseDto
        {
            IdUsuario = usuario.IdUsuario,
            NombreCompleto = $"{usuario.Persona.Nombres} {usuario.Persona.Apellidos}",
            CorreoElectronico = usuario.Persona.CorreoElectronico,
            Token = token
        };
    }
}
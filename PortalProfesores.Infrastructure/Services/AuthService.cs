using Microsoft.EntityFrameworkCore;
using PortalProfesores.Domain.DTOs.Auth;
using PortalProfesores.Domain.Entities;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Domain.Interfaces.Services;
using PortalProfesores.Infrastructure.Data;
using PortalProfesores.Domain.Exceptions;


namespace PortalProfesores.Infrastructure.Services;

/// <summary>
/// Servicio responsable de la autenticación y registro de usuarios.
/// Centraliza la lógica de negocio relacionada con el acceso al sistema.
/// </summary>
public class AuthService : IAuthService
{
    private readonly PortalProfesoresDbContext _context;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPersonaRepository _personaRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;
    

    public AuthService(
        PortalProfesoresDbContext context,
        IUsuarioRepository usuarioRepository,
        IPersonaRepository personaRepository,
        IPasswordService passwordService,
        IJwtService jwtService)
    {
        _context = context;
        _usuarioRepository = usuarioRepository;
        _personaRepository = personaRepository;
        _passwordService = passwordService;
        _jwtService = jwtService;
        
    }

    /// <summary>
    /// Autentica un usuario y genera un token JWT.
    /// </summary>
    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        // Validar información de entrada.
        if (string.IsNullOrWhiteSpace(request.CorreoElectronico))
            throw new ValidationException(
                "Debe ingresar un correo electrónico.");

        // Buscar el usuario registrado.
        var usuario = await _usuarioRepository.ObtenerPorCorreoAsync(request.CorreoElectronico);

        if (usuario == null)
            throw new UnauthorizedException(
                "Correo electrónico o contraseña incorrectos.");

        // Validar estado del usuario.
        if (!usuario.EstadoUsuario.Nombre.Equals("Activo", StringComparison.OrdinalIgnoreCase))
            throw new UnauthorizedException("El usuario no se encuentra activo.");

        if (string.IsNullOrWhiteSpace(usuario.ClaveHash))
            throw new ValidationException("La cuenta no tiene una contraseña configurada.");

        // Validar contraseña.
        if (!_passwordService.VerifyPassword(request.Clave, usuario.ClaveHash))
            throw new UnauthorizedException("Correo electrónico o contraseña incorrectos.");

        // Registrar el último acceso.
        usuario.UltimoAcceso = DateTime.UtcNow;

        await _usuarioRepository.ActualizarAsync(usuario);

        // Generar JWT.
        var token = _jwtService.GenerarToken(
            usuario.IdUsuario,
            usuario.Persona.CorreoElectronico,
            $"{usuario.Persona.Nombres} {usuario.Persona.Apellidos}");

        return new LoginResponseDto
        {
            IdUsuario = usuario.IdUsuario,
            NombreCompleto = $"{usuario.Persona.Nombres} {usuario.Persona.Apellidos}",
            CorreoElectronico = usuario.Persona.CorreoElectronico,
            Token = token
        };
    }

    /// <summary>
    /// Registra un nuevo usuario en la plataforma.
    /// La operación se ejecuta dentro de una transacción para
    /// garantizar la consistencia de los datos.
    /// </summary>
    public async Task<RegisterResponseDto> RegistrarAsync(RegisterRequestDto request)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            // Validar información obligatoria.
            if (string.IsNullOrWhiteSpace(request.Nombres) ||
                string.IsNullOrWhiteSpace(request.Apellidos) ||
                string.IsNullOrWhiteSpace(request.CorreoElectronico) ||
                string.IsNullOrWhiteSpace(request.Clave))
            {
                throw new ValidationException("Todos los campos son obligatorios.");
            }

            // Verificar que el correo no exista.
            if (await _usuarioRepository.ExisteCorreoAsync(request.CorreoElectronico))
            {
                throw new ConflictException("El correo electrónico ya se encuentra registrado.");
            }

            // Crear la persona.
            var persona = new Persona
            {
                IdTipoDocumento = 1,     // Temporal
                IdCiudad = 3,            // Temporal
                NumeroDocumento = Guid.NewGuid().ToString(),

                Nombres = request.Nombres,
                Apellidos = request.Apellidos,
                CorreoElectronico = request.CorreoElectronico,
                FechaIngreso = DateTime.UtcNow
        };

            await _personaRepository.CrearAsync(persona);
            // PRUEBA
            await _context.GuardarCambiosAsync();

            // Crear el usuario asociado.
            var usuario = new Usuario
            {
                Persona = persona,
                IdEstadoUsuario = 1, // Estado Activo
                ClaveHash = _passwordService.HashPassword(request.Clave)
            };

            await _usuarioRepository.CrearAsync(usuario);

            // Persistir todos los cambios de la transacción.
            await _context.GuardarCambiosAsync();

            // Confirmar la transacción.
            await transaction.CommitAsync();

            return new RegisterResponseDto
            {
                IdUsuario = usuario.IdUsuario,
                Mensaje = "Usuario registrado correctamente."
            };
        }
        catch
        {
            // Revertir todos los cambios si ocurre algún error.
            await transaction.RollbackAsync();
            throw;
        }
    }

    //public async Task<PerfilResponseDto> ObtenerPerfilAsync(long idUsuario)
    //{
    //    var usuario = await _usuarioRepository.ObtenerPorIdAsync(idUsuario);

    //    if (usuario == null)
    //        throw new Exception("Usuario no encontrado.");

    //    return new PerfilResponseDto
    //    {
    //        IdUsuario = usuario.IdUsuario,
    //        Nombres = usuario.Persona.Nombres,
    //        Apellidos = usuario.Persona.Apellidos,
    //        CorreoElectronico = usuario.Persona.CorreoElectronico,
    //        Estado = usuario.EstadoUsuario.Nombre
    //    };
    //}
}
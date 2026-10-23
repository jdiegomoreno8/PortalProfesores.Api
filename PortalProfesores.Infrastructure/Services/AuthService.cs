using Microsoft.EntityFrameworkCore;
using PortalProfesores.Domain.DTOs.Auth;
using PortalProfesores.Domain.Entities;
using PortalProfesores.Domain.Exceptions;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Domain.Interfaces.Services;
using PortalProfesores.Infrastructure.Data;

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
        if (string.IsNullOrWhiteSpace(request.CorreoElectronico))
        {
            throw new ValidationException(
                "Debe ingresar un correo electrónico.");
        }

        var usuario = await _usuarioRepository
            .ObtenerPorCorreoAsync(request.CorreoElectronico);

        if (usuario == null)
        {
            throw new UnauthorizedException(
                "Correo electrónico o contraseña incorrectos.");
        }

        if (!usuario.EstadoUsuario.Nombre.Equals(
                "Activo",
                StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedException(
                "El usuario no se encuentra activo.");
        }

        if (string.IsNullOrWhiteSpace(usuario.ClaveHash))
        {
            throw new ValidationException(
                "La cuenta no tiene una contraseña configurada.");
        }

        if (!_passwordService.VerifyPassword(
                request.Clave,
                usuario.ClaveHash))
        {
            throw new UnauthorizedException(
                "Correo electrónico o contraseña incorrectos.");
        }

        usuario.UltimoAcceso = DateTime.UtcNow;

        await _usuarioRepository.ActualizarAsync(usuario);

        var roles = usuario.Roles
       .Where(x => x.Rol != null)
       .Select(x => x.Rol.Nombre)
       .ToList();

        var token = _jwtService.GenerarToken(
            usuario.IdUsuario,
            usuario.Persona!.CorreoElectronico,
            $"{usuario.Persona.Nombres} {usuario.Persona.Apellidos}",
            roles);

        return new LoginResponseDto
        {
            IdUsuario = usuario.IdUsuario,
            NombreCompleto =
                $"{usuario.Persona.Nombres} {usuario.Persona.Apellidos}",
            CorreoElectronico = usuario.Persona.CorreoElectronico,
            Token = token
        };
    }

    /// <summary>
    /// Registra un nuevo usuario como estudiante.
    /// El rol de estudiante se asigna automáticamente.
    /// </summary>
    public async Task<RegisterResponseDto> RegistrarAsync(
        RegisterRequestDto request)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            // Validar campos obligatorios.
            if (string.IsNullOrWhiteSpace(request.Nombres) ||
                string.IsNullOrWhiteSpace(request.Apellidos) ||
                string.IsNullOrWhiteSpace(request.NumeroDocumento) ||
                string.IsNullOrWhiteSpace(request.CorreoElectronico) ||
                string.IsNullOrWhiteSpace(request.Clave))
            {
                throw new ValidationException(
                    "Todos los campos son obligatorios.");
            }

            // Validar tipo de documento.
            var tipoDocumento = await _context.TiposDocumento
                .FirstOrDefaultAsync(x =>
                    x.IdTipoDocumento == request.IdTipoDocumento);

            if (tipoDocumento == null)
            {
                throw new ValidationException(
                    "El tipo de documento seleccionado no es válido.");
            }

            // Verificar correo existente.
            if (await _usuarioRepository
                .ExisteCorreoAsync(request.CorreoElectronico))
            {
                throw new ConflictException(
                    "El correo electrónico ya se encuentra registrado.");
            }

            // Verificar documento existente.
            var documentoExiste = await _context.Personas
                .AnyAsync(x =>
                    x.IdTipoDocumento == request.IdTipoDocumento &&
                    x.NumeroDocumento == request.NumeroDocumento.Trim());

            if (documentoExiste)
            {
                throw new ConflictException(
                    "El número de documento ya se encuentra registrado.");
            }

            //Nivel

            var nivelExiste = await _context.Niveles
                .AnyAsync(x => x.IdNivel == request.IdNivel);

            if (!nivelExiste)
            {
                throw new ValidationException(
                    "El nivel académico seleccionado no es válido.");
            }


            // Crear Persona.
            var persona = new Persona
            {
                IdTipoDocumento = request.IdTipoDocumento,
                IdCiudad = null,
                NumeroDocumento = request.NumeroDocumento.Trim(),

                Nombres = request.Nombres.Trim(),
                Apellidos = request.Apellidos.Trim(),
                CorreoElectronico =
                    request.CorreoElectronico.Trim(),

                FechaIngreso = DateTime.UtcNow
            };

            await _personaRepository.CrearAsync(persona);

            await _context.GuardarCambiosAsync();

            // Crear Usuario.
            var usuario = new Usuario
            {
                Persona = persona,
                IdEstadoUsuario = 1,
                ClaveHash =
                    _passwordService.HashPassword(request.Clave)
            };

            await _usuarioRepository.CrearAsync(usuario);

            await _context.GuardarCambiosAsync();

            // El registro público siempre crea un Estudiante.
            var usuarioRol = new UsuarioRol
            {
                IdUsuario = usuario.IdUsuario,
                IdRol = 3
            };

            _context.UsuariosRoles.Add(usuarioRol);

            await _context.GuardarCambiosAsync();

            // Crear el perfil del estudiante.
            var estudiante = new EstudianteDetalle
            {
                IdPersona = persona.IdPersona,
                IdNivel = request.IdNivel,
                ObjetivoAprendizaje = null,
                Observaciones = null,
                FotoPerfilUrl = null,
                FechaIngreso = DateTime.UtcNow,
                FechaCreacion = DateTime.UtcNow
            };

            _context.Set<EstudianteDetalle>().Add(estudiante);

            await _context.GuardarCambiosAsync();



            await transaction.CommitAsync();

            return new RegisterResponseDto
            {
                IdUsuario = usuario.IdUsuario,
                Mensaje = "Usuario registrado correctamente."
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }


    }


}
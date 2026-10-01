using PortalProfesores.Domain.DTOs.Profesores;
using PortalProfesores.Domain.Entities;
using PortalProfesores.Domain.Exceptions;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Domain.Interfaces.Services;
using PortalProfesores.Infrastructure.Data;

namespace PortalProfesores.Infrastructure.Services;

/// <summary>
/// Servicio encargado de las operaciones administrativas
/// relacionadas con la gestión de profesores.
/// </summary>
public class AdministradorService : IAdministradorService
{
    private const long EstadoUsuarioActivo = 1;
    private const long EstadoUsuarioSuspendido = 2;
    private const long EstadoUsuarioBloqueado = 3;
    private const long EstadoUsuarioEliminado = 4;

    private const long RolProfesor = 2;

    private readonly IPersonaRepository _personaRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IProfesorDetalleRepository _profesorDetalleRepository;
    private readonly IPasswordService _passwordService;
    private readonly PortalProfesoresDbContext _context;
    private readonly IProfesorRepository _profesorRepository;

    public AdministradorService(
        IPersonaRepository personaRepository,
        IUsuarioRepository usuarioRepository,
        IProfesorDetalleRepository profesorDetalleRepository,
        IProfesorRepository profesorRepository,
        IPasswordService passwordService,
        PortalProfesoresDbContext context)
    {
        _personaRepository = personaRepository;
        _usuarioRepository = usuarioRepository;
        _profesorDetalleRepository = profesorDetalleRepository;
        _profesorRepository = profesorRepository;
        _passwordService = passwordService;
        _context = context;
    }

    public async Task CrearProfesorAsync(CrearProfesorRequestDto request)
    {
        ValidarRequest(request);

        var correo = request.CorreoElectronico.Trim();

        if (await _usuarioRepository.ExisteCorreoAsync(correo))
        {
            throw new ConflictException(
                "El correo electrónico ya se encuentra registrado.");
        }

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var ahora = DateTime.UtcNow;

            // 1. Crear la persona.
            var persona = new Persona
            {
                IdTipoDocumento = request.IdTipoDocumento,
                IdCiudad = request.IdCiudad,
                NumeroDocumento = request.NumeroDocumento.Trim(),
                Nombres = request.Nombres.Trim(),
                Apellidos = request.Apellidos.Trim(),
                CorreoElectronico = correo,
                Telefono = string.IsNullOrWhiteSpace(request.Telefono)
                    ? null
                    : request.Telefono.Trim(),
                FechaNacimiento = request.FechaNacimiento,
                FechaIngreso = ahora
            };

            await _personaRepository.CrearAsync(persona);

            // 2. Crear el usuario relacionado con la persona.
            var usuario = new Usuario
            {
                IdEstadoUsuario = EstadoUsuarioActivo,
                ClaveHash = _passwordService.HashPassword(request.Clave),
                Persona = persona
            };

            // El rol se asigna internamente.
            usuario.Roles.Add(new UsuarioRol
            {
                IdRol = RolProfesor,
                Usuario = usuario
            });

            await _usuarioRepository.CrearAsync(usuario);

            // 3. Crear el detalle del profesor relacionado
            // directamente con la misma persona.
            var profesorDetalle = new ProfesorDetalle
            {
                Persona = persona,

                Presentacion = string.IsNullOrWhiteSpace(
                    request.Presentacion)
                    ? null
                    : request.Presentacion.Trim(),

                Experiencia = string.IsNullOrWhiteSpace(
                    request.Experiencia)
                    ? null
                    : request.Experiencia.Trim(),

                Biografia = string.IsNullOrWhiteSpace(
                    request.Biografia)
                    ? null
                    : request.Biografia.Trim(),

                FechaIngreso = ahora,
                FechaCreacion = ahora
            };

            await _profesorDetalleRepository.CrearAsync(
                profesorDetalle);

            // EF Core resuelve las claves Identity y las
            // relaciones entre las entidades al guardar.
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private static void ValidarRequest(CrearProfesorRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Nombres))
            throw new ArgumentException("Debe ingresar el nombre.");

        if (request.Nombres.Trim().Length > 200)
            throw new ArgumentException(
                "El nombre no puede superar los 200 caracteres.");

        if (string.IsNullOrWhiteSpace(request.Apellidos))
            throw new ArgumentException("Debe ingresar los apellidos.");

        if (request.Apellidos.Trim().Length > 200)
            throw new ArgumentException(
                "Los apellidos no pueden superar los 200 caracteres.");

        if (request.IdTipoDocumento <= 0)
            throw new ArgumentException(
                "Debe seleccionar un tipo de documento.");

        if (string.IsNullOrWhiteSpace(request.NumeroDocumento))
            throw new ArgumentException(
                "Debe ingresar el número de documento.");

        if (request.NumeroDocumento.Trim().Length > 120)
            throw new ArgumentException(
                "El número de documento no puede superar los 120 caracteres.");

        if (string.IsNullOrWhiteSpace(request.CorreoElectronico))
            throw new ArgumentException(
                "Debe ingresar el correo electrónico.");

        if (!request.CorreoElectronico.Contains("@"))
            throw new ArgumentException(
                "El correo electrónico no es válido.");

        if (string.IsNullOrWhiteSpace(request.Clave))
            throw new ArgumentException(
                "Debe ingresar la contraseña.");

        if (request.Clave.Length < 6)
            throw new ArgumentException(
                "La contraseña debe tener al menos 6 caracteres.");

        if (!string.IsNullOrWhiteSpace(request.Telefono)
            && request.Telefono.Trim().Length > 60)
        {
            throw new ArgumentException(
                "El teléfono no puede superar los 60 caracteres.");
        }

        if (request.FechaNacimiento >
            DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new ArgumentException(
                "La fecha de nacimiento no es válida.");
        }

        if (!string.IsNullOrWhiteSpace(request.Presentacion)
            && request.Presentacion.Length > 4000)
        {
            throw new ArgumentException(
                "La presentación no puede superar los 4000 caracteres.");
        }

        if (!string.IsNullOrWhiteSpace(request.Experiencia)
            && request.Experiencia.Length > 4000)
        {
            throw new ArgumentException(
                "La experiencia no puede superar los 4000 caracteres.");
        }

        if (!string.IsNullOrWhiteSpace(request.Biografia)
            && request.Biografia.Length > 4000)
        {
            throw new ArgumentException(
                "La biografía no puede superar los 4000 caracteres.");
        }
    }

    /// <summary>
    /// Obtiene el listado de profesores para la gestión administrativa.
    /// </summary>
    public async Task<IEnumerable<ProfesorListadoDto>> ObtenerProfesoresAsync()
    {
        return await _profesorRepository.ObtenerListadoAsync();
    }

    /// <summary>
    /// Obtiene el detalle completo de un profesor.
    /// </summary>
    public async Task<ProfesorDetalleDto?> ObtenerDetalleProfesorAsync(
        long idProfesor)
    {
        if (idProfesor <= 0)
        {
            return null;
        }

        return await _profesorRepository.ObtenerDetalleAsync(idProfesor);
    }

    /// <summary>
    /// Actualiza la información editable de un profesor.
    /// </summary>
    public async Task EditarProfesorAsync(
        long idProfesor,
        EditarProfesorRequestDto request)
    {
        if (idProfesor <= 0)
        {
            throw new ArgumentException(
                "El identificador del profesor no es válido.");
        }

        ValidarEdicion(request);

        var profesorDetalle =
            await _profesorDetalleRepository.ObtenerPorIdAsync(idProfesor);

        if (profesorDetalle is null)
        {
            throw new KeyNotFoundException(
                "El profesor no fue encontrado.");
        }

        if (profesorDetalle.Persona is null)
        {
            throw new InvalidOperationException(
                "El profesor no tiene una persona asociada.");
        }

        var persona = profesorDetalle.Persona;

        if (persona.Usuario is null)
        {
            throw new InvalidOperationException(
                "El profesor no tiene un usuario asociado.");
        }

        var usuario = persona.Usuario;

        var correo = request.CorreoElectronico.Trim();

        if (!string.Equals(
            persona.CorreoElectronico,
            correo,
            StringComparison.OrdinalIgnoreCase))
        {
            if (await _usuarioRepository.ExisteCorreoAsync(
                correo,
                usuario.IdUsuario))
            {
                throw new ConflictException(
                    "El correo electrónico ya se encuentra registrado.");
            }
        }

        var numeroDocumento = request.NumeroDocumento.Trim();

        if (!string.Equals(
            persona.NumeroDocumento,
            numeroDocumento,
            StringComparison.OrdinalIgnoreCase))
        {
            if (await _personaRepository.ExisteDocumentoAsync(
                numeroDocumento,
                persona.IdPersona))
            {
                throw new ConflictException(
                    "El número de documento ya se encuentra registrado.");
            }
        }

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            // Actualizar Persona.
            persona.IdTipoDocumento = request.IdTipoDocumento;
            persona.IdCiudad = request.IdCiudad;
            persona.NumeroDocumento = request.NumeroDocumento.Trim();
            persona.Nombres = request.Nombres.Trim();
            persona.Apellidos = request.Apellidos.Trim();
            persona.CorreoElectronico = correo;

            persona.Telefono = string.IsNullOrWhiteSpace(request.Telefono)
                ? null
                : request.Telefono.Trim();

            persona.FechaNacimiento = request.FechaNacimiento;

            // Actualizar información profesional.
            profesorDetalle.Presentacion =
                string.IsNullOrWhiteSpace(request.Presentacion)
                    ? null
                    : request.Presentacion.Trim();

            profesorDetalle.Experiencia =
                string.IsNullOrWhiteSpace(request.Experiencia)
                    ? null
                    : request.Experiencia.Trim();

            profesorDetalle.Biografia =
                string.IsNullOrWhiteSpace(request.Biografia)
                    ? null
                    : request.Biografia.Trim();

            profesorDetalle.FechaActualizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// Cambia el estado de un profesor.
    /// </summary>
    public async Task CambiarEstadoProfesorAsync(
        long idProfesor,
        CambiarEstadoProfesorRequestDto request)
    {
        if (idProfesor <= 0)
        {
            throw new ArgumentException(
                "El identificador del profesor no es válido.");
        }

        if (request.IdEstadoUsuario <= 0)
        {
            throw new ArgumentException(
                "El estado seleccionado no es válido.");
        }

        if (request.IdEstadoUsuario > EstadoUsuarioEliminado)
        {
            throw new ArgumentException(
                "El estado seleccionado no es válido.");
        }

        var profesorDetalle =
            await _profesorDetalleRepository.ObtenerPorIdAsync(idProfesor);

        if (profesorDetalle is null)
        {
            throw new KeyNotFoundException(
                "El profesor no fue encontrado.");
        }

        if (profesorDetalle.Persona is null)
        {
            throw new InvalidOperationException(
                "El profesor no tiene una persona asociada.");
        }

        if (profesorDetalle.Persona.Usuario is null)
        {
            throw new InvalidOperationException(
                "El profesor no tiene un usuario asociado.");
        }

        var usuario = profesorDetalle.Persona.Usuario;

        var estadoActual = usuario.IdEstadoUsuario;
        var nuevoEstado = request.IdEstadoUsuario;

        if (estadoActual == nuevoEstado)
        {
            throw new ArgumentException(
                "El profesor ya tiene seleccionado ese estado.");
        }

        ValidarCambioEstado(estadoActual, nuevoEstado);

        usuario.IdEstadoUsuario = nuevoEstado;

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Valida las transiciones permitidas entre estados.
    /// </summary>
    private static void ValidarCambioEstado(
        long estadoActual,
        long nuevoEstado)
    {
        switch (estadoActual)
        {
            case EstadoUsuarioActivo:

                if (nuevoEstado != EstadoUsuarioSuspendido &&
                    nuevoEstado != EstadoUsuarioBloqueado &&
                    nuevoEstado != EstadoUsuarioEliminado)
                {
                    throw new ArgumentException(
                        "El cambio de estado seleccionado no está permitido.");
                }

                break;

            case EstadoUsuarioSuspendido:

                if (nuevoEstado != EstadoUsuarioActivo &&
                    nuevoEstado != EstadoUsuarioBloqueado &&
                    nuevoEstado != EstadoUsuarioEliminado)
                {
                    throw new ArgumentException(
                        "El cambio de estado seleccionado no está permitido.");
                }

                break;

            case EstadoUsuarioBloqueado:

                if (nuevoEstado != EstadoUsuarioActivo &&
                    nuevoEstado != EstadoUsuarioSuspendido &&
                    nuevoEstado != EstadoUsuarioEliminado)
                {
                    throw new ArgumentException(
                        "El cambio de estado seleccionado no está permitido.");
                }

                break;

            case EstadoUsuarioEliminado:

                throw new ArgumentException(
                    "Un profesor eliminado no puede cambiar de estado.");

            default:

                throw new ArgumentException(
                    "El estado actual del profesor no es válido.");
        }
    }


    /// <summary>
    /// Valida la información editable de un profesor.
    /// </summary>
    private static void ValidarEdicion(
        EditarProfesorRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Nombres))
            throw new ArgumentException(
                "Debe ingresar el nombre.");

        if (request.Nombres.Trim().Length > 200)
            throw new ArgumentException(
                "El nombre no puede superar los 200 caracteres.");

        if (string.IsNullOrWhiteSpace(request.Apellidos))
            throw new ArgumentException(
                "Debe ingresar los apellidos.");

        if (request.Apellidos.Trim().Length > 200)
            throw new ArgumentException(
                "Los apellidos no pueden superar los 200 caracteres.");

        if (request.IdTipoDocumento <= 0)
            throw new ArgumentException(
                "Debe seleccionar un tipo de documento.");

        if (string.IsNullOrWhiteSpace(request.NumeroDocumento))
            throw new ArgumentException(
                "Debe ingresar el número de documento.");

        if (request.NumeroDocumento.Trim().Length > 120)
            throw new ArgumentException(
                "El número de documento no puede superar los 120 caracteres.");

        if (string.IsNullOrWhiteSpace(request.CorreoElectronico))
            throw new ArgumentException(
                "Debe ingresar el correo electrónico.");

        if (!request.CorreoElectronico.Contains("@"))
            throw new ArgumentException(
                "El correo electrónico no es válido.");

        if (!string.IsNullOrWhiteSpace(request.Telefono)
            && request.Telefono.Trim().Length > 60)
        {
            throw new ArgumentException(
                "El teléfono no puede superar los 60 caracteres.");
        }

        if (request.FechaNacimiento >
            DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new ArgumentException(
                "La fecha de nacimiento no es válida.");
        }

        if (!string.IsNullOrWhiteSpace(request.Presentacion)
            && request.Presentacion.Length > 4000)
        {
            throw new ArgumentException(
                "La presentación no puede superar los 4000 caracteres.");
        }

        if (!string.IsNullOrWhiteSpace(request.Experiencia)
            && request.Experiencia.Length > 4000)
        {
            throw new ArgumentException(
                "La experiencia no puede superar los 4000 caracteres.");
        }

        if (!string.IsNullOrWhiteSpace(request.Biografia)
            && request.Biografia.Length > 4000)
        {
            throw new ArgumentException(
                "La biografía no puede superar los 4000 caracteres.");
        }
    }
}
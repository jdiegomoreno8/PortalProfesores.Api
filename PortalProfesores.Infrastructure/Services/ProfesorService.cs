using PortalProfesores.Domain.DTOs.Profesores;
using PortalProfesores.Domain.Exceptions;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Domain.Interfaces.Services;

namespace PortalProfesores.Infrastructure.Services;

/// <summary>
/// Servicio encargado de administrar la información
/// del perfil del profesor.
/// </summary>
public class ProfesorService : IProfesorService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IProfesorDetalleRepository _profesorDetalleRepository;
    private readonly IFotoPerfilStorageService _fotoPerfilStorageService;
    private readonly ICiudadRepository _ciudadRepository;

    public ProfesorService(
            IUsuarioRepository usuarioRepository,
    IProfesorDetalleRepository profesorDetalleRepository,
    IFotoPerfilStorageService fotoPerfilStorageService,
    ICiudadRepository ciudadRepository)
    {
        _usuarioRepository = usuarioRepository;
        _profesorDetalleRepository = profesorDetalleRepository;
        _fotoPerfilStorageService = fotoPerfilStorageService;
        _ciudadRepository = ciudadRepository;

    }


    /// <summary>
    /// Obtiene la información del profesor autenticado.
    /// </summary>
    public async Task<ProfesorPerfilResponseDto> ObtenerPerfilAsync(
        long idUsuario)
    {
        var usuario = await _usuarioRepository
            .ObtenerPorIdAsync(idUsuario);

        if (usuario == null)
        {
            throw new NotFoundException(
                "No se encontró la información del profesor.");
        }

        if (usuario.Persona == null)
        {
            throw new NotFoundException(
                "No se encontró la información personal del profesor.");
        }

        var persona = usuario.Persona;

        var profesorDetalle =
            await _profesorDetalleRepository
                .ObtenerPorPersonaAsync(persona.IdPersona);

        return new ProfesorPerfilResponseDto
        {
            IdUsuario = usuario.IdUsuario,
            Nombres = persona.Nombres,
            Apellidos = persona.Apellidos,
            CorreoElectronico = persona.CorreoElectronico,
            Telefono = persona.Telefono,
            FechaNacimiento = persona.FechaNacimiento,
            IdCiudad = persona.IdCiudad,
            Ciudad = persona.Ciudad?.Nombre,
            Estado = usuario.EstadoUsuario.Nombre,
            FechaIngreso = persona.FechaIngreso,
            FotoPerfilUrl = profesorDetalle?.FotoPerfilUrl
        };
    }




    /// <summary>
    /// Foto de perfil
    /// </summary>
    public async Task CambiarFotoPerfilAsync(
        long idUsuario,
        Stream archivo)
    {
        var usuario = await _usuarioRepository
            .ObtenerPorIdAsync(idUsuario);

        if (usuario == null || usuario.Persona == null)
        {
            throw new NotFoundException(
                "No se encontró la información personal del profesor.");
        }

        var profesorDetalle =
            await _profesorDetalleRepository
                .ObtenerPorPersonaAsync(usuario.Persona.IdPersona);

        if (profesorDetalle == null)
        {
            throw new NotFoundException(
                "No se encontró el perfil del profesor.");
        }

        var rutaFotoAnterior = profesorDetalle.FotoPerfilUrl;

        var nombreProfesor =
            $"{usuario.Persona.Nombres} {usuario.Persona.Apellidos}";

        var nuevaFotoPerfilUrl =
            await _fotoPerfilStorageService.GuardarAsync(
                archivo,
                nombreProfesor,
                "profesores");

        try
        {
            profesorDetalle.FotoPerfilUrl = nuevaFotoPerfilUrl;

            await _profesorDetalleRepository
                .ActualizarAsync(profesorDetalle);
        }
        catch
        {
            // Si falla la actualización, intentamos eliminar
            // el archivo nuevo para evitar archivos huérfanos.
            try
            {
                await _fotoPerfilStorageService.EliminarAsync(
                    nuevaFotoPerfilUrl,
                    "profesores");
            }
            catch
            {
                // No ocultamos la excepción original.
            }

            throw;
        }

        // La base de datos ya apunta a la nueva fotografía.
        // Eliminamos la anterior únicamente después de actualizar.
        if (!string.IsNullOrWhiteSpace(rutaFotoAnterior))
        {
            await _fotoPerfilStorageService.EliminarAsync(
                rutaFotoAnterior,
                "profesores");
        }
    }



    /// <summary>
    /// Actualiza la información del profesor.
    /// </summary>
    public async Task ActualizarPerfilAsync(
        long idUsuario,
        ActualizarProfesorRequestDto request)
    {
        // Validar nombres.
        if (string.IsNullOrWhiteSpace(request.Nombres))
        {
            throw new ArgumentException(
                "Debe ingresar el nombre.");
        }

        if (request.Nombres.Trim().Length > 200)
        {
            throw new ArgumentException(
                "El nombre no puede superar los 200 caracteres.");
        }

        // Validar apellidos.
        if (string.IsNullOrWhiteSpace(request.Apellidos))
        {
            throw new ArgumentException(
                "Debe ingresar los apellidos.");
        }

        if (request.Apellidos.Trim().Length > 200)
        {
            throw new ArgumentException(
                "Los apellidos no pueden superar los 200 caracteres.");
        }

        // Validar correo.
        if (string.IsNullOrWhiteSpace(request.CorreoElectronico))
        {
            throw new ArgumentException(
                "Debe ingresar el correo electrónico.");
        }

        if (!request.CorreoElectronico.Contains("@"))
        {
            throw new ArgumentException(
                "El correo electrónico no es válido.");
        }

        // Validar ciudad.
        if (request.IdCiudad == null)
        {
            throw new ArgumentException(
                "Debe seleccionar una ciudad.");
        }

        var ciudad = await _ciudadRepository
    .ObtenerPorIdAsync(request.IdCiudad.Value);

        if (ciudad == null)
        {
            throw new ArgumentException(
                "La ciudad seleccionada no existe.");
        }

        // Validar fecha de nacimiento.
        if (request.FechaNacimiento == null)
        {
            throw new ArgumentException(
                "Debe ingresar la fecha de nacimiento.");
        }

        if (request.FechaNacimiento > DateOnly.FromDateTime(DateTime.Today))
        {
            throw new ArgumentException(
                "La fecha de nacimiento no es válida.");
        }

        // Validar teléfono.
        if (!string.IsNullOrWhiteSpace(request.Telefono)
            && request.Telefono.Trim().Length > 60)
        {
            throw new ArgumentException(
                "El teléfono no puede superar los 60 caracteres.");
        }

        // Verificar que el correo no pertenezca a otro usuario.
        var correo = request.CorreoElectronico.Trim();

        if (await _usuarioRepository.ExisteCorreoAsync(
            correo,
            idUsuario))
        {
            throw new ConflictException(
                "El correo electrónico ya se encuentra registrado.");
        }

        var usuario = await _usuarioRepository
            .ObtenerPorIdAsync(idUsuario);

        if (usuario == null)
        {
            throw new NotFoundException(
                "No se encontró la información del profesor.");
        }

        if (usuario.Persona == null)
        {
            throw new NotFoundException(
                "No se encontró la información personal del profesor.");
        }

        usuario.Persona.Nombres = request.Nombres.Trim();
        usuario.Persona.Apellidos = request.Apellidos.Trim();
        usuario.Persona.CorreoElectronico = correo;
        usuario.Persona.Telefono = request.Telefono?.Trim();
        usuario.Persona.FechaNacimiento = request.FechaNacimiento.Value;
        usuario.Persona.IdCiudad = request.IdCiudad.Value;

        await _usuarioRepository.ActualizarAsync(usuario);
    }

}
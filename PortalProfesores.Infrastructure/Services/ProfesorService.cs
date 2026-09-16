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

    public ProfesorService(
        IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    /// <summary>
    /// Obtiene la información del profesor autenticado.
    /// </summary>
    public async Task<ProfesorPerfilResponseDto> ObtenerPerfilAsync(
        long idUsuario)
    {
        var usuario = await _usuarioRepository.ObtenerPorIdAsync(idUsuario);

        if (usuario == null)
        {
            throw new Exception("No se encontró la información del profesor.");
        }

        return new ProfesorPerfilResponseDto
        {
            IdUsuario = usuario.IdUsuario,
            Nombres = usuario.Persona.Nombres,
            Apellidos = usuario.Persona.Apellidos,
            CorreoElectronico = usuario.Persona.CorreoElectronico,
            Estado = usuario.EstadoUsuario.Nombre,
            FechaIngreso = usuario.Persona.FechaIngreso
        };
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

        if (request.Nombres.Length > 200)
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

        if (request.Apellidos.Length > 200)
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

        //Validad ciudad
        if (request.IdCiudad == null)
        {
            throw new ArgumentException(
                "Debe seleccionar una ciudad.");
        }

        //Fecha de nacimiento
        if (request.FechaNacimiento == null)
        {
            throw new ArgumentException(
                "Debe ingresar la fecha de nacimiento.");
        }

        //Validación fecha nacimiento
        if (request.FechaNacimiento > DateOnly.FromDateTime(DateTime.Today))
        {
            throw new ArgumentException(
                "La fecha de nacimiento no es válida.");
        }

        // Validar teléfono.
        if (!string.IsNullOrWhiteSpace(request.Telefono)
            && request.Telefono.Length > 60)
        {
            throw new ArgumentException(
                "El teléfono no puede superar los 30 caracteres.");
        }



        // Verificar que el correo no pertenezca a otro usuario.
        if (await _usuarioRepository.ExisteCorreoAsync(
            request.CorreoElectronico,
            idUsuario))
        {
            throw new ConflictException(
                "El correo electrónico ya se encuentra registrado.");
        }

        var usuario = await _usuarioRepository.ObtenerPorIdAsync(idUsuario);

        if (usuario == null)
        {
            throw new Exception(
                "No se encontró la información del profesor.");
        }

        usuario.Persona.Nombres = request.Nombres.Trim();

        usuario.Persona.Apellidos = request.Apellidos.Trim();

        usuario.Persona.CorreoElectronico =
            request.CorreoElectronico.Trim();

        usuario.Persona.Telefono =
            request.Telefono.Trim();

        usuario.Persona.FechaNacimiento =
            request.FechaNacimiento;

        //usuario.Persona.IdCiudad =
        //    request.IdCiudad;

        await _usuarioRepository.ActualizarAsync(usuario);
    }
}
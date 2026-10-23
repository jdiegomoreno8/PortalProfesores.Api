using Microsoft.EntityFrameworkCore;
using PortalProfesores.Domain.DTOs.Profesores;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Infrastructure.Data;

namespace PortalProfesores.Infrastructure.Repositories;

/// <summary>
/// Repositorio para las consultas relacionadas
/// con los profesores.
/// </summary>
public class ProfesorRepository : IProfesorRepository
{
    private readonly PortalProfesoresDbContext _context;

    public ProfesorRepository(PortalProfesoresDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene la información necesaria para el
    /// listado administrativo de profesores.
    /// </summary>
    public async Task<IEnumerable<ProfesorListadoDto>> ObtenerListadoAsync()
    {
        return await _context.ProfesoresDetalle
            .AsNoTracking()
            .Select(profesor => new ProfesorListadoDto
            {
                IdProfesor = profesor.IdProfesor,
                IdPersona = profesor.IdPersona,

                Nombres = profesor.Persona.Nombres,
                Apellidos = profesor.Persona.Apellidos,

                CorreoElectronico =
                    profesor.Persona.CorreoElectronico,

                Telefono =
                    profesor.Persona.Telefono,

                IdCiudad = profesor.Persona.IdCiudad,

                Ciudad =
                    profesor.Persona.Ciudad != null
                        ? profesor.Persona.Ciudad.Nombre
                        : null,

                FechaIngreso = profesor.FechaIngreso,

                Estado =
                    profesor.Persona.Usuario != null &&
                    profesor.Persona.Usuario.EstadoUsuario != null
                        ? profesor.Persona.Usuario.EstadoUsuario.Nombre
                        : string.Empty
            })
            .OrderBy(profesor => profesor.Apellidos)
            .ThenBy(profesor => profesor.Nombres)
            .ToListAsync();
    }
    public async Task<ProfesorDetalleDto?> ObtenerDetalleAsync(long idProfesor)
    {
        return await _context.ProfesoresDetalle
            .AsNoTracking()
            .Where(profesor => profesor.IdProfesor == idProfesor)
            .Select(profesor => new ProfesorDetalleDto
            {
                IdProfesor = profesor.IdProfesor,
                IdPersona = profesor.IdPersona,

                IdTipoDocumento = profesor.Persona.IdTipoDocumento,

                TipoDocumento = profesor.Persona.TipoDocumento != null
        ? profesor.Persona.TipoDocumento.Nombre
        : string.Empty,

                NumeroDocumento = profesor.Persona.NumeroDocumento,

                Nombres = profesor.Persona.Nombres,
                Apellidos = profesor.Persona.Apellidos,
                CorreoElectronico = profesor.Persona.CorreoElectronico,
                Telefono = profesor.Persona.Telefono,
                FechaNacimiento = profesor.Persona.FechaNacimiento,

                IdCiudad = profesor.Persona.IdCiudad,

                Ciudad = profesor.Persona.Ciudad != null
        ? profesor.Persona.Ciudad.Nombre
        : null,

                Estado = profesor.Persona.Usuario != null &&
             profesor.Persona.Usuario.EstadoUsuario != null
        ? profesor.Persona.Usuario.EstadoUsuario.Nombre
        : string.Empty,

                Rol = profesor.Persona.Usuario != null
        ? profesor.Persona.Usuario.Roles
            .Select(ur => ur.Rol.Nombre)
            .FirstOrDefault() ?? string.Empty
        : string.Empty,


                Presentacion = profesor.Presentacion,
                Experiencia = profesor.Experiencia,
                Biografia = profesor.Biografia,
                FotoPerfilUrl = profesor.FotoPerfilUrl,
                FechaIngreso = profesor.FechaIngreso

            })
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Obtiene la información pública de los profesores
    /// activos para la Landing.
    /// </summary>
    public async Task<IEnumerable<ProfesorPublicoDto>>
        ObtenerProfesoresPublicosAsync()
    {
        const long estadoUsuarioActivo = 1;

        return await _context.ProfesoresDetalle
            .AsNoTracking()
            .Where(profesor =>
                profesor.Persona.Usuario != null &&
                profesor.Persona.Usuario.IdEstadoUsuario ==
                    estadoUsuarioActivo)
            .Select(profesor => new ProfesorPublicoDto
            {
                IdProfesor = profesor.IdProfesor,

                Nombres = profesor.Persona.Nombres,

                Apellidos = profesor.Persona.Apellidos,

                Ciudad = profesor.Persona.Ciudad != null
                    ? profesor.Persona.Ciudad.Nombre
                    : null,

                Presentacion = profesor.Presentacion,

                Experiencia = profesor.Experiencia,

                Biografia = profesor.Biografia,

                FotoPerfilUrl = profesor.FotoPerfilUrl,
            })
        .OrderBy(_ => Guid.NewGuid())
.Take(3)
.ToListAsync();
    }

}
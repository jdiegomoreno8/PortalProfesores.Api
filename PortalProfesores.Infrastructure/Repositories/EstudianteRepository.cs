using Microsoft.EntityFrameworkCore;
using PortalProfesores.Domain.DTOs.Estudiantes;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Infrastructure.Data;

namespace PortalProfesores.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de estudiantes
/// utilizando Entity Framework Core.
/// </summary>
public class EstudianteRepository : IEstudianteRepository
{
    private readonly PortalProfesoresDbContext _context;

    public EstudianteRepository(
        PortalProfesoresDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene la información completa de un estudiante.
    ///
    /// La consulta obtiene información relacionada de:
    /// - Persona
    /// - Tipo de documento
    /// - Ciudad
    /// - Usuario
    /// - Estado del usuario
    /// - Rol
    /// - Nivel académico
    ///
    /// El resultado se proyecta directamente al DTO.
    /// De esta manera no exponemos la Entity fuera
    /// de la capa de infraestructura.
    /// </summary>
    public async Task<EstudianteDetalleDto?> ObtenerPorIdAsync(
        long idEstudiante)
    {
        return await _context.EstudiantesDetalle
            .AsNoTracking()
            .Where(estudiante =>
                estudiante.IdEstudiante == idEstudiante)
            .Select(estudiante => new EstudianteDetalleDto
            {
                // -------------------------------------------------
                // IDENTIFICACIÓN
                // -------------------------------------------------

                IdEstudiante = estudiante.IdEstudiante,

                IdPersona = estudiante.IdPersona,

                // -------------------------------------------------
                // DATOS PERSONALES
                // -------------------------------------------------

                IdTipoDocumento =
                    estudiante.Persona.IdTipoDocumento,

                TipoDocumento =
                    estudiante.Persona.TipoDocumento != null
                        ? estudiante.Persona.TipoDocumento.Nombre
                        : string.Empty,

                NumeroDocumento =
                    estudiante.Persona.NumeroDocumento,

                Nombres =
                    estudiante.Persona.Nombres,

                Apellidos =
                    estudiante.Persona.Apellidos,

                CorreoElectronico =
                    estudiante.Persona.CorreoElectronico,

                Telefono =
                    estudiante.Persona.Telefono,

                FechaNacimiento =
                    estudiante.Persona.FechaNacimiento,

                IdCiudad =
                    estudiante.Persona.IdCiudad,

                Ciudad =
                    estudiante.Persona.Ciudad != null
                        ? estudiante.Persona.Ciudad.Nombre
                        : null,

                // -------------------------------------------------
                // ESTADO DE LA CUENTA
                // -------------------------------------------------

                IdEstadoUsuario =
    estudiante.Persona.Usuario != null
        ? estudiante.Persona.Usuario.IdEstadoUsuario
        : 0,

                Estado =
    estudiante.Persona.Usuario != null &&
    estudiante.Persona.Usuario.EstadoUsuario != null
        ? estudiante.Persona.Usuario
            .EstadoUsuario.Nombre
        : string.Empty,

                Rol =
    estudiante.Persona.Usuario != null
        ? estudiante.Persona.Usuario.Roles
            .Select(ur => ur.Rol.Nombre)
            .FirstOrDefault() ?? string.Empty
        : string.Empty,
                // -------------------------------------------------
                // INFORMACIÓN ACADÉMICA
                // -------------------------------------------------

                IdNivel =
                    estudiante.IdNivel,

                Nivel =
                    estudiante.Nivel != null
                        ? estudiante.Nivel.Nombre
                        : null,

                ObjetivoAprendizaje =
                    estudiante.ObjetivoAprendizaje,

                Observaciones =
                    estudiante.Observaciones,

                // -------------------------------------------------
                // FECHA DE INGRESO
                // -------------------------------------------------

                FechaIngreso =
                    estudiante.FechaIngreso
            })
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Obtiene la información resumida de los estudiantes
    /// para el listado administrativo.
    ///
    /// La consulta obtiene únicamente los campos necesarios
    /// para representar cada estudiante en la tabla.
    /// </summary>
    public async Task<IEnumerable<EstudianteListadoDto>>
        ObtenerListadoAsync()
    {
        return await _context.EstudiantesDetalle
            .AsNoTracking()
            .Select(estudiante => new EstudianteListadoDto
            {
                // -------------------------------------------------
                // IDENTIFICACIÓN
                // -------------------------------------------------

                IdEstudiante =
                    estudiante.IdEstudiante,

                IdPersona =
                    estudiante.IdPersona,

                // -------------------------------------------------
                // DATOS PERSONALES
                // -------------------------------------------------

                Nombres =
                    estudiante.Persona.Nombres,

                Apellidos =
                    estudiante.Persona.Apellidos,

                CorreoElectronico =
                    estudiante.Persona.CorreoElectronico,

                Telefono =
                    estudiante.Persona.Telefono,

                Ciudad =
                    estudiante.Persona.Ciudad != null
                        ? estudiante.Persona.Ciudad.Nombre
                        : null,

                // -------------------------------------------------
                // INFORMACIÓN ACADÉMICA
                // -------------------------------------------------

                Nivel =
                    estudiante.Nivel != null
                        ? estudiante.Nivel.Nombre
                        : null,

                // -------------------------------------------------
                // ESTADO
                // -------------------------------------------------

                Estado =
                    estudiante.Persona.Usuario != null &&
                    estudiante.Persona.Usuario.EstadoUsuario != null
                        ? estudiante.Persona.Usuario
                            .EstadoUsuario.Nombre
                        : string.Empty,

                // -------------------------------------------------
                // FECHA DE INGRESO
                // -------------------------------------------------

                FechaIngreso =
                    estudiante.FechaIngreso
            })
            .OrderBy(estudiante => estudiante.Apellidos)
            .ThenBy(estudiante => estudiante.Nombres)
            .ToListAsync();
    }

    /// <summary>
    /// Actualiza la información personal y académica
    /// de un estudiante.
    ///
    /// La información personal se actualiza sobre la entidad
    /// Persona relacionada con el estudiante y la información
    /// académica sobre EstudianteDetalle.
    /// </summary>
    public async Task<bool> EditarAsync(
        long idEstudiante,
        EditarEstudianteRequest request)
    {
        // -------------------------------------------------
        // BUSCAR ESTUDIANTE
        // -------------------------------------------------

        var estudiante = await _context.EstudiantesDetalle
            .Include(e => e.Persona)
            .FirstOrDefaultAsync(e =>
                e.IdEstudiante == idEstudiante);

        // Si no existe el estudiante, no hay nada
        // que actualizar.
        if (estudiante is null)
        {
            return false;
        }

        Console.WriteLine(
    $"Estudiante: {estudiante.IdEstudiante}");

        Console.WriteLine(
            $"Persona: {estudiante.IdPersona}");

        Console.WriteLine(
            $"Documento actual: {estudiante.Persona.NumeroDocumento}");

        Console.WriteLine(
            $"Estado Persona EF: " +
            $"{_context.Entry(estudiante.Persona).State}");

        // -------------------------------------------------
        // DATOS PERSONALES
        // -------------------------------------------------

        estudiante.Persona.Nombres =
            request.Nombres;

        estudiante.Persona.Apellidos =
            request.Apellidos;

        estudiante.Persona.IdTipoDocumento =
            request.IdTipoDocumento;

        estudiante.Persona.NumeroDocumento =
            request.NumeroDocumento;

        estudiante.Persona.CorreoElectronico =
            request.CorreoElectronico;

        estudiante.Persona.Telefono =
            request.Telefono;

        estudiante.Persona.FechaNacimiento =
       request.FechaNacimiento.HasValue
           ? DateOnly.FromDateTime(request.FechaNacimiento.Value)
           : null;

        estudiante.Persona.IdCiudad =
            request.IdCiudad;

        // -------------------------------------------------
        // INFORMACIÓN ACADÉMICA
        // -------------------------------------------------

        estudiante.IdNivel =
            request.IdNivel;

        estudiante.ObjetivoAprendizaje =
            request.ObjetivoAprendizaje;

        estudiante.Observaciones =
            request.Observaciones;

        // -------------------------------------------------
        // GUARDAR CAMBIOS
        // -------------------------------------------------

        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Cambia el estado del usuario asociado al estudiante.
    /// </summary>
    public async Task<bool> CambiarEstadoAsync(
        long idEstudiante,
        long idEstadoUsuario)
    {
        var estudiante = await _context.EstudiantesDetalle
            .Include(e => e.Persona)
                .ThenInclude(p => p.Usuario)
            .FirstOrDefaultAsync(e =>
                e.IdEstudiante == idEstudiante);

        if (estudiante is null)
        {
            return false;
        }

        if (estudiante.Persona.Usuario is null)
        {
            return false;
        }

        estudiante.Persona.Usuario.IdEstadoUsuario =
            idEstadoUsuario;

        await _context.SaveChangesAsync();

        return true;
    }

}
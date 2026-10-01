using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Domain.Interfaces.Repositories;

/// <summary>
/// Define las operaciones de acceso a datos relacionadas
/// con el detalle del perfil del profesor.
/// </summary>
public interface IProfesorDetalleRepository
{
    /// <summary>
    /// Obtiene el detalle de un profesor a partir del identificador
    /// de la persona asociada.
    /// </summary>
    Task<ProfesorDetalle?> ObtenerPorPersonaAsync(long idPersona);

    /// <summary>
    /// Obtiene el detalle de un profesor a partir de su identificador.
    /// Incluye la persona y el usuario relacionado.
    /// </summary>
    Task<ProfesorDetalle?> ObtenerPorIdAsync(long idProfesor);

    /// <summary>
    /// Registra el detalle de un nuevo profesor.
    /// </summary>
    Task<ProfesorDetalle> CrearAsync(ProfesorDetalle profesorDetalle);

    /// <summary>
    /// Actualiza el detalle de un profesor existente.
    /// </summary>
    Task ActualizarAsync(ProfesorDetalle profesorDetalle);
}
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Domain.Interfaces.Repositories;

/// <summary>
/// Define las operaciones de acceso a datos relacionadas
/// con los estudiantes.
/// </summary>
public interface IEstudianteRepository
{
    /// <summary>
    /// Obtiene un estudiante a partir de su identificador.
    /// Incluye la información de la Persona y el Nivel académico.
    /// </summary>
    Task<EstudianteDetalle?> ObtenerPorIdAsync(long idEstudiante);
}
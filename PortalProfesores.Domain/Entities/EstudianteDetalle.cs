namespace PortalProfesores.Domain.Entities;

/// <summary>
/// Contiene la información específica del perfil de un estudiante.
///
/// Los datos personales permanecen en Persona.
/// Esta entidad almacena información académica y administrativa
/// propia del estudiante.
/// </summary>
public class EstudianteDetalle
{
    /// <summary>
    /// Identificador único del estudiante.
    /// </summary>
    public long IdEstudiante { get; set; }

    /// <summary>
    /// Identificador de la Persona asociada.
    /// </summary>
    public long IdPersona { get; set; }

    /// <summary>
    /// Nivel académico actual del estudiante.
    /// Se relaciona con el catálogo Nivel.
    /// </summary>
    public long IdNivel { get; set; }

    /// <summary>
    /// Objetivo principal que tiene el estudiante
    /// dentro de su proceso de aprendizaje.
    /// </summary>
    public string? ObjetivoAprendizaje { get; set; }

    /// <summary>
    /// Observaciones adicionales relacionadas
    /// con el perfil o proceso del estudiante.
    /// </summary>
    public string? Observaciones { get; set; }
    /// <summary>
    /// Foto de perfil url
    public string? FotoPerfilUrl { get; set; }

    /// <summary>
    /// Fecha en la que se registra el estudiante.
    /// </summary>
    public DateTime FechaIngreso { get; set; }

    /// <summary>
    /// Fecha de creación del registro.
    /// </summary>
    public DateTime FechaCreacion { get; set; }

    /// <summary>
    /// Usuario que creó el registro.
    /// </summary>
    public long? UsuarioCreacion { get; set; }

    /// <summary>
    /// Fecha de la última actualización.
    /// </summary>
    public DateTime? FechaActualizacion { get; set; }

    /// <summary>
    /// Usuario que realizó la última actualización.
    /// </summary>
    public long? UsuarioActualizacion { get; set; }

    /// <summary>
    /// Persona asociada al estudiante.
    /// </summary>
    public Persona Persona { get; set; } = null!;

    /// <summary>
    /// Nivel académico asociado.
    /// </summary>
    public Nivel? Nivel { get; set; }
}
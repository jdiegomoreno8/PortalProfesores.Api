namespace PortalProfesores.Domain.Entities;

/// <summary>
/// Representa un nivel académico del estudiante.
///
/// Actualmente el catálogo contiene niveles como A1 y A2.
/// Esta entidad corresponde a la tabla Nivel de la base de datos.
/// </summary>
public class Nivel
{
    /// <summary>
    /// Identificador único del nivel.
    /// Corresponde a la clave primaria de la tabla Nivel.
    /// </summary>
    public long IdNivel { get; set; }

    /// <summary>
    /// Nombre del nivel.
    /// Ejemplo: A1, A2.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Descripción opcional del nivel.
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Fecha en la que se creó el registro del catálogo.
    /// </summary>
    public DateTime FechaCreacion { get; set; }

    /// <summary>
    /// Usuario que creó el registro.
    /// </summary>
    public long? UsuarioCreacion { get; set; }

    /// <summary>
    /// Fecha de la última actualización del registro.
    /// </summary>
    public DateTime? FechaActualizacion { get; set; }

    /// <summary>
    /// Usuario que realizó la última actualización.
    /// </summary>
    public long? UsuarioActualizacion { get; set; }
}
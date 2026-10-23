namespace PortalProfesores.Domain.DTOs.Estudiantes;

/// <summary>
/// Información completa de un estudiante.
/// 
/// Este DTO combina los datos personales provenientes
/// de Persona con la información académica propia
/// de EstudianteDetalle.
/// 
/// Se utiliza para consultas del módulo de estudiantes.
/// </summary>
public class EstudianteDetalleDto
{
    // ---------------------------------------------------------
    // IDENTIFICACIÓN
    // ---------------------------------------------------------

    /// <summary>
    /// Identificador único del estudiante.
    /// </summary>
    public long IdEstudiante { get; set; }

    /// <summary>
    /// Identificador de la persona asociada.
    /// </summary>
    public long IdPersona { get; set; }

    /// <summary>
    /// Identificador del tipo de documento.
    /// </summary>
    public long IdTipoDocumento { get; set; }

    /// <summary>
    /// Nombre del tipo de documento.
    /// </summary>
    public string TipoDocumento { get; set; } = string.Empty;

    /// <summary>
    /// Número de documento del estudiante.
    /// </summary>
    public string NumeroDocumento { get; set; } = string.Empty;


    // ---------------------------------------------------------
    // DATOS PERSONALES
    // ---------------------------------------------------------

    /// <summary>
    /// Nombres del estudiante.
    /// </summary>
    public string Nombres { get; set; } = string.Empty;

    /// <summary>
    /// Apellidos del estudiante.
    /// </summary>
    public string Apellidos { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del estudiante.
    /// </summary>
    public string CorreoElectronico { get; set; } = string.Empty;

    /// <summary>
    /// Teléfono del estudiante.
    /// </summary>
    public string? Telefono { get; set; }

    /// <summary>
    /// Fecha de nacimiento del estudiante.
    /// </summary>
    public DateOnly? FechaNacimiento { get; set; }

    /// <summary>
    /// Identificador de la ciudad.
    /// </summary>
    public long? IdCiudad { get; set; }

    /// <summary>
    /// Nombre de la ciudad.
    /// </summary>
    public string? Ciudad { get; set; }


    // ---------------------------------------------------------
    // ESTADO DE LA CUENTA
    // ---------------------------------------------------------

    /// <summary>
    /// Identificador del estado actual del usuario.
    /// </summary>
    public long IdEstadoUsuario { get; set; }

    /// <summary>
    /// Estado actual de la cuenta del estudiante.
    /// </summary>
    public string Estado { get; set; } = string.Empty;
       
    /// <summary>
    /// Rol asociado al usuario.
    /// </summary>
    public string Rol { get; set; } = string.Empty;


    // ---------------------------------------------------------
    // INFORMACIÓN ACADÉMICA
    // ---------------------------------------------------------

    /// <summary>
    /// Identificador del nivel académico.
    /// Puede ser NULL si todavía no se ha definido.
    /// </summary>
    public long? IdNivel { get; set; }

    /// <summary>
    /// Nombre del nivel académico.
    /// Ejemplo: A1, A2.
    /// </summary>
    public string? Nivel { get; set; }

    /// <summary>
    /// Objetivo principal del estudiante
    /// dentro de su proceso de aprendizaje.
    /// </summary>
    public string? ObjetivoAprendizaje { get; set; }

    /// <summary>
    /// Observaciones relacionadas con el estudiante.
    /// </summary>
    public string? Observaciones { get; set; }
    // ---------------------------------------------------------
    // Foto url
    // ---------------------------------------------------------
    public string? FotoPerfilUrl { get; set; }


    // ---------------------------------------------------------
    // FECHAS
    // ---------------------------------------------------------

    /// <summary>
    /// Fecha en la que el estudiante ingresó al sistema.
    /// </summary>
    public DateTime FechaIngreso { get; set; }
}
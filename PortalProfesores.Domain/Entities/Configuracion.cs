
namespace PortalProfesores.Domain.Entities;

/// <summary>
/// Representa una configuración general del portal.
/// Los valores se administran desde la base de datos.
/// </summary>
public class Configuracion
{
    public long IdConfiguracion { get; set; }

    public string Clave { get; set; } = string.Empty;

    public string Valor { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public string TipoDato { get; set; } = string.Empty;

    public DateTime FechaCreacion { get; set; }

    public string? UsuarioCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? UsuarioActualizacion { get; set; }
}

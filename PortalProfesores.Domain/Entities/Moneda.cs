namespace PortalProfesores.Domain.Entities;

/// <summary>
/// Representa una moneda utilizada para registrar pagos
/// dentro del portal.
/// </summary>
public class Moneda
{
    public long IdMoneda { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string CodigoISO { get; set; } = string.Empty;

    public string? Simbolo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public long? UsuarioActualizacion { get; set; }

    // Relaciones
    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
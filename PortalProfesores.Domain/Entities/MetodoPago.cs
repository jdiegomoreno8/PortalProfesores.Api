namespace PortalProfesores.Domain.Entities;

/// <summary>
/// Representa un método de pago disponible en el portal.
/// </summary>
public class MetodoPago
{
    public long IdMetodoPago { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public long? UsuarioActualizacion { get; set; }

    // Relaciones
    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
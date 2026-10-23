namespace PortalProfesores.Domain.Entities;

/// <summary>
/// Representa los estados posibles de un pago.
/// </summary>
public class EstadoPago
{
    public long IdEstadoPago { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public long? UsuarioActualizacion { get; set; }

    // Relaciones
    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
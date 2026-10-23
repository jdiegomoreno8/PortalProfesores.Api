namespace PortalProfesores.Domain.Entities;

/// <summary>
/// Representa el pago asociado a una reserva.
/// </summary>
public class Pago
{
    public long IdPago { get; set; }

    public long IdReserva { get; set; }

    public long IdEstadoPago { get; set; }

    public long IdMetodoPago { get; set; }

    public long IdMoneda { get; set; }

    public decimal ValorOriginal { get; set; }

    public decimal? TasaCambio { get; set; }

    public decimal? ValorConvertido { get; set; }

    public DateTime FechaInicioPago { get; set; }

    public DateTime? FechaPago { get; set; }

    public string? IdTransaccionExterna { get; set; }

    public string? Observacion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public long? UsuarioActualizacion { get; set; }

    // Relaciones
    public Reserva? Reserva { get; set; }

    public EstadoPago? EstadoPago { get; set; }

    public MetodoPago? MetodoPago { get; set; }

    public Moneda? Moneda { get; set; }
}

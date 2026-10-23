namespace PortalProfesores.Domain.Entities;

/// <summary>
/// Representa los estados posibles de una reserva.
/// </summary>
public class EstadoReserva
{
    public long IdEstadoReserva { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public long? UsuarioActualizacion { get; set; }

    // Relaciones
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}

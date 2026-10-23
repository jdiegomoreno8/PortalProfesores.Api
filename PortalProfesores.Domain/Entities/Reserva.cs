
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalProfesores.Domain.Entities;

/// <summary>
/// Representa la reserva de una disponibilidad por parte de una persona.
/// </summary>
public class Reserva
{
    [Key]
    public long IdReserva { get; set; }

    public long IdDisponibilidad { get; set; }

    public long IdPersona { get; set; }

    public long IdEstadoReserva { get; set; }

    public DateTime FechaReserva { get; set; }  

    public DateTime? FechaExpiracion { get; set; }

    [MaxLength(1000)]
    public string? Observacion { get; set; }
   

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public long? UsuarioActualizacion { get; set; }

    // Relaciones

    [ForeignKey(nameof(IdDisponibilidad))]
    public Disponibilidad? Disponibilidad { get; set; }

    [ForeignKey(nameof(IdPersona))]
    public Persona? Persona { get; set; }

    [ForeignKey(nameof(IdEstadoReserva))]
    public EstadoReserva? EstadoReserva { get; set; }

    public Sesion? Sesion { get; set; }
}

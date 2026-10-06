using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalProfesores.Domain.Entities;

/// <summary>
/// Representa una sesión realizada a partir de una clase reservada.
/// </summary>
public class Sesion
{
    [Key]
    public long IdSesion { get; set; }

    public long IdReserva { get; set; }

    public long IdClase { get; set; }

    public DateTime? FechaInicioReal { get; set; }

    public DateTime? FechaFinReal { get; set; }

    public string? ObservacionesProfesor { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public long? UsuarioActualizacion { get; set; }


    // Relaciones

    [ForeignKey(nameof(IdClase))]
    public Clase? Clase { get; set; }
}
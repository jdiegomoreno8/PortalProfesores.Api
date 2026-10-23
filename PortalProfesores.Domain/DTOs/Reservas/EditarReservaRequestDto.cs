
using System.ComponentModel.DataAnnotations;

namespace PortalProfesores.Domain.DTOs.Reservas;

/// <summary>
/// Datos permitidos para editar una reserva.
/// No permite modificar el estado ni el pago.
/// </summary>
public class EditarReservaRequestDto
{
    [Range(1, long.MaxValue,
        ErrorMessage = "Debe seleccionar una disponibilidad válida.")]
    public long IdDisponibilidad { get; set; }
}

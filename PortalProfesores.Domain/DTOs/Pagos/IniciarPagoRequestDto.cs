
using System.ComponentModel.DataAnnotations;

namespace PortalProfesores.Application.DTOs.Pagos;

/// <summary>
/// Datos necesarios para iniciar el pago de una reserva.
/// El precio y la moneda se obtienen desde la clase asociada.
/// </summary>
public class IniciarPagoRequestDto
{
    [Range(1, long.MaxValue,
        ErrorMessage = "Debe indicar una reserva válida.")]
    public long IdReserva { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace PortalProfesores.Domain.DTOs.Reservas;

/// <summary>
/// Datos necesarios para solicitar la creación de una reserva.
/// </summary>
public class CrearReservaRequestDto
{
    /// <summary>
    /// Identificador del horario que se desea reservar.
    /// </summary>
    [Range(1, long.MaxValue,
        ErrorMessage = "Debe indicar una disponibilidad válida.")]
    public long IdDisponibilidad { get; set; }

    /// <summary>
    /// Identificador de la clase que se desea reservar.
    /// </summary>

    [Range(1, long.MaxValue,
    ErrorMessage = "Debe indicar una clase válida.")]
    public long IdClase { get; set; }

    /// <summary>
    /// Identificador de la persona que solicita la reserva.
    /// Se asigna desde el usuario autenticado en el backend.
    /// </summary>
    [Range(1, long.MaxValue,
        ErrorMessage = "Debe indicar una persona válida.")]
    public long? IdPersona { get; set; }

    /// <summary>
    /// Observación opcional de la reserva.
    /// </summary>
    [MaxLength(500,
        ErrorMessage = "La observación no puede superar los 500 caracteres.")]
    public string? Observacion { get; set; }
}

namespace PortalProfesores.Domain.Interfaces.Services;

/// <summary>
/// Define el contrato común para las pasarelas de pago.
/// </summary>
public interface IPasarelaPagoService
{
    /// <summary>
    /// Inicia una operación de pago en la pasarela configurada.
    /// </summary>
    Task<ResultadoPasarelaPago> ProcesarPagoAsync(
        SolicitudPasarelaPago solicitud,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Información mínima necesaria para procesar un pago.
/// </summary>
public sealed record SolicitudPasarelaPago(
    long IdPago,
    decimal Valor,
    string CodigoMoneda,
    string Referencia);

/// <summary>
/// Resultado devuelto por la pasarela.
/// </summary>
public sealed record ResultadoPasarelaPago(
    EstadoResultadoPasarela Estado,
    string? IdTransaccionExterna,
    string Mensaje);

/// <summary>
/// Resultados posibles de una operación de pago.
/// </summary>
public enum EstadoResultadoPasarela
{
    Pendiente = 1,
    Aprobado = 2,
    Rechazado = 3,
    Error = 4
}

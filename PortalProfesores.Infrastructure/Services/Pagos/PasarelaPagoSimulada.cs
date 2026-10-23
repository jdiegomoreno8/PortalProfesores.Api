
using Microsoft.Extensions.Configuration;
using PortalProfesores.Domain.Interfaces.Services;

namespace PortalProfesores.Infrastructure.Services.Pagos;

/// <summary>
/// Simula una pasarela de pagos para desarrollo y pruebas.
/// No realiza cobros ni se conecta a proveedores externos.
/// </summary>
public sealed class PasarelaPagoSimulada : IPasarelaPagoService
{
    private readonly IConfiguration _configuration;

    public PasarelaPagoSimulada(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<ResultadoPasarelaPago> ProcesarPagoAsync(
        SolicitudPasarelaPago solicitud,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (solicitud.IdPago <= 0)
        {
            throw new ArgumentException(
                "El identificador del pago debe ser mayor que cero.",
                nameof(solicitud));
        }

        if (solicitud.Valor <= 0)
        {
            throw new ArgumentException(
                "El valor del pago debe ser mayor que cero.",
                nameof(solicitud));
        }

        if (string.IsNullOrWhiteSpace(solicitud.CodigoMoneda))
        {
            throw new ArgumentException(
                "El código de moneda es obligatorio.",
                nameof(solicitud));
        }

        if (string.IsNullOrWhiteSpace(solicitud.Referencia))
        {
            throw new ArgumentException(
                "La referencia del pago es obligatoria.",
                nameof(solicitud));
        }

        var resultadoConfigurado =
            _configuration["PasarelaPagoSimulada:Resultado"] ?? "Pendiente";

        if (!Enum.TryParse<EstadoResultadoPasarela>(
                resultadoConfigurado,
                ignoreCase: true,
                out var estado) ||
            !Enum.IsDefined(estado))
        {
            throw new InvalidOperationException(
                "El resultado configurado para la pasarela simulada no es válido.");
        }

        var mensaje = estado switch
        {
            EstadoResultadoPasarela.Pendiente =>
                "El pago simulado está pendiente de confirmación.",

            EstadoResultadoPasarela.Aprobado =>
                "El pago simulado fue aprobado.",

            EstadoResultadoPasarela.Rechazado =>
                "El pago simulado fue rechazado.",

            EstadoResultadoPasarela.Error =>
                "Se produjo un error simulado al procesar el pago.",

            _ => throw new InvalidOperationException(
                "El resultado de la pasarela no está soportado.")
        };

        string? idTransaccionExterna =
            estado is EstadoResultadoPasarela.Aprobado
                or EstadoResultadoPasarela.Rechazado
            ? $"SIM-{Guid.NewGuid():N}"
            : null;

        var resultado = new ResultadoPasarelaPago(
            estado,
            idTransaccionExterna,
            mensaje);

        return Task.FromResult(resultado);
    }
}

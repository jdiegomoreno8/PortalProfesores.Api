
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PortalProfesores.Domain.Interfaces.Repositories;

namespace PortalProfesores.Infrastructure.BackgroundServices;

/// <summary>
/// Ejecuta periódicamente la expiración de reservas
/// pendientes de pago cuyo plazo ha vencido.
/// </summary>
public class ReservaVencimientoService : BackgroundService
{
    private static readonly TimeSpan IntervaloRevision =
        TimeSpan.FromMinutes(1);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ReservaVencimientoService> _logger;

    public ReservaVencimientoService(
        IServiceScopeFactory scopeFactory,
        ILogger<ReservaVencimientoService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(IntervaloRevision);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcesarReservasVencidasAsync();
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error al procesar las reservas vencidas.");
            }

            try
            {
                if (!await timer.WaitForNextTickAsync(stoppingToken))
                {
                    break;
                }
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
        }
    }

    /// <summary>
    /// Crea un ámbito de dependencias y delega
    /// la expiración al repositorio de reservas.
    /// </summary>
    private async Task ProcesarReservasVencidasAsync()
    {
        await using var scope = _scopeFactory.CreateAsyncScope();

        var reservaRepository = scope.ServiceProvider
            .GetRequiredService<IReservaRepository>();

        var cantidadExpirada =
            await reservaRepository.ExpirarReservasVencidasAsync();

        if (cantidadExpirada > 0)
        {
            _logger.LogInformation(
                "Se expiraron {Cantidad} reservas vencidas.",
                cantidadExpirada);
        }
    }
}

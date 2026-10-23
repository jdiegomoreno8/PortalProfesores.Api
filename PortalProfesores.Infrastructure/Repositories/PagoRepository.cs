
using Microsoft.EntityFrameworkCore;
using PortalProfesores.Domain.Entities;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Infrastructure.Data;

namespace PortalProfesores.Infrastructure.Repositories;

/// <summary>
/// Implementa las operaciones de persistencia de pagos.
/// </summary>
public class PagoRepository : IPagoRepository
{
    private readonly PortalProfesoresDbContext _context;

    public PagoRepository(PortalProfesoresDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene un pago por su identificador.
    /// </summary>
    public async Task<Pago?> ObtenerPorIdAsync(long idPago)
    {
        return await _context.Pagos
            .AsNoTracking()
            .Include(p => p.EstadoPago)
            .Include(p => p.MetodoPago)
            .Include(p => p.Moneda)
            .FirstOrDefaultAsync(p => p.IdPago == idPago);
    }

    /// <summary>
    /// Obtiene el pago asociado a una reserva.
    /// </summary>
    public async Task<Pago?> ObtenerPorReservaAsync(long idReserva)
    {
        return await _context.Pagos
            .AsNoTracking()
            .Include(p => p.EstadoPago)
            .Include(p => p.MetodoPago)
            .Include(p => p.Moneda)
            .Where(p => p.IdReserva == idReserva)
            .OrderByDescending(p => p.FechaCreacion)
            .ThenByDescending(p => p.IdPago)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Registra un nuevo pago.
    /// </summary>
    public async Task<Pago> CrearAsync(Pago pago)
    {
        await _context.Pagos.AddAsync(pago);
        await _context.SaveChangesAsync();

        return pago;
    }

    /// <summary>
    /// Actualiza un pago existente.
    /// </summary>
    public async Task<Pago> ActualizarAsync(Pago pago)
    {
        var pagoExistente = await _context.Pagos
            .FirstOrDefaultAsync(p => p.IdPago == pago.IdPago);

        if (pagoExistente is null)
        {
            throw new KeyNotFoundException(
                $"No se encontró el pago con identificador {pago.IdPago}.");
        }

        pagoExistente.IdEstadoPago = pago.IdEstadoPago;
        pagoExistente.FechaPago = pago.FechaPago;
        pagoExistente.IdTransaccionExterna = pago.IdTransaccionExterna;
        pagoExistente.Observacion = pago.Observacion;
        pagoExistente.TasaCambio = pago.TasaCambio;
        pagoExistente.ValorConvertido = pago.ValorConvertido;
        pagoExistente.FechaActualizacion = DateTime.UtcNow;
        pagoExistente.UsuarioActualizacion = pago.UsuarioActualizacion;

        await _context.SaveChangesAsync();

        return pagoExistente;
    }


    /// <summary>
    /// Actualiza un pago sin guardar inmediatamente los cambios.
    /// El servicio controla cuándo se persisten dentro de la transacción.
    /// </summary>
    public async Task<Pago> ActualizarSinGuardarAsync(Pago pago)
    {
        var pagoExistente = await _context.Pagos
            .FirstOrDefaultAsync(p => p.IdPago == pago.IdPago);

        if (pagoExistente is null)
        {
            throw new KeyNotFoundException(
                $"No se encontró el pago con identificador {pago.IdPago}.");
        }

        pagoExistente.IdEstadoPago = pago.IdEstadoPago;
        pagoExistente.FechaPago = pago.FechaPago;
        pagoExistente.IdTransaccionExterna = pago.IdTransaccionExterna;
        pagoExistente.Observacion = pago.Observacion;
        pagoExistente.TasaCambio = pago.TasaCambio;
        pagoExistente.ValorConvertido = pago.ValorConvertido;
        pagoExistente.FechaActualizacion = DateTime.UtcNow;
        pagoExistente.UsuarioActualizacion = pago.UsuarioActualizacion;

        return pagoExistente;
    }

}

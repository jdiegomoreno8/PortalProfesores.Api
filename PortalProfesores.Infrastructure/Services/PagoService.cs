
using Microsoft.EntityFrameworkCore;
using PortalProfesores.Domain.Entities;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Domain.Interfaces.Services;
using PortalProfesores.Infrastructure.Data;

namespace PortalProfesores.Infrastructure.Services;

/// <summary>
/// Gestiona las operaciones de negocio relacionadas con los pagos.
/// </summary>
public class PagoService : IPagoService
{

    private readonly IPagoRepository _pagoRepository;
    private readonly IReservaRepository _reservaRepository;
    private readonly IPasarelaPagoService _pasarelaPagoService;
    private readonly PortalProfesoresDbContext _context;

    public PagoService(
        IPagoRepository pagoRepository,
        IReservaRepository reservaRepository,
        IPasarelaPagoService pasarelaPagoService,
        PortalProfesoresDbContext context)
    {
        _pagoRepository = pagoRepository;
        _reservaRepository = reservaRepository;
        _pasarelaPagoService = pasarelaPagoService;
        _context = context;
    }


    /// <summary>
    /// Obtiene un pago por su identificador.
    /// </summary>
    public async Task<Pago?> ObtenerPorIdAsync(long idPago)
    {
        if (idPago <= 0)
        {
            throw new ArgumentException(
                "El identificador del pago debe ser mayor que cero.",
                nameof(idPago));
        }

        return await _pagoRepository.ObtenerPorIdAsync(idPago);
    }

    /// <summary>
    /// Consulta el pago más reciente asociado a una reserva.
    /// </summary>
    public async Task<Pago?> ObtenerPorReservaAsync(long idReserva)
    {
        if (idReserva <= 0)
        {
            throw new ArgumentException(
                "El identificador de la reserva debe ser mayor que cero.",
                nameof(idReserva));
        }

        return await _pagoRepository.ObtenerPorReservaAsync(idReserva);
    }

    /// <summary>
    /// Inicia el pago de una reserva.
    /// La implementación de negocio se incorporará en el siguiente bloque.
    /// </summary>

    public async Task<Pago> IniciarPagoAsync(
        long idReserva,
        long idPersona)
    {
        if (idReserva <= 0)
        {
            throw new ArgumentException(
                "El identificador de la reserva debe ser mayor que cero.",
                nameof(idReserva));
        }

        if (idPersona <= 0)
        {
            throw new ArgumentException(
                "El identificador de la persona debe ser mayor que cero.",
                nameof(idPersona));
        }

        var reserva = await _reservaRepository
            .ObtenerPorIdAsync(idReserva);

        if (reserva is null)
        {
            throw new KeyNotFoundException(
                "La reserva indicada no existe.");
        }

        if (reserva.IdPersona != idPersona)
        {
            throw new UnauthorizedAccessException(
                "No tienes permiso para pagar esta reserva.");
        }

        if (!string.Equals(
            reserva.EstadoReserva?.Nombre,
            "PendientePago",
            StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "La reserva no está pendiente de pago.");
        }

        if (reserva.FechaExpiracion is null ||
            reserva.FechaExpiracion.Value <= DateTime.UtcNow)
        {
            throw new InvalidOperationException(
                "El plazo para pagar esta reserva ha vencido.");
        }

        if (reserva.Sesion?.Clase is null)
        {
            throw new InvalidOperationException(
                "La reserva no tiene una clase asociada.");
        }

        if (reserva.Sesion.Clase.Precio <= 0)
        {
            throw new InvalidOperationException(
                "El precio de la clase no es válido.");
        }

        if (reserva.Sesion.Clase.Moneda is null)
        {
            throw new InvalidOperationException(
                "No se encontró la moneda de la clase.");
        }

        // Consultar el intento de pago más reciente.
        var pagoExistente = await _pagoRepository
            .ObtenerPorReservaAsync(idReserva);

        if (pagoExistente is not null)
        {
            var estadoPago = pagoExistente.EstadoPago?.Nombre;

            if (string.Equals(
                    estadoPago,
                    "Aprobado",
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Esta reserva ya tiene un pago aprobado.");
            }

            if (string.Equals(
                    estadoPago,
                    "Pendiente",
                    StringComparison.OrdinalIgnoreCase) ||
                string.Equals(
                    estadoPago,
                    "Procesando",
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "La reserva ya tiene un pago pendiente o en proceso.");
            }
        }

        var estadoPagoPendiente = await _context.EstadosPago
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Nombre == "Pendiente");

        if (estadoPagoPendiente is null)
        {
            throw new InvalidOperationException(
                "No está configurado el estado de pago Pendiente.");
        }

        var metodoPasarela = await _context.MetodosPago
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Nombre == "Pasarela");

        if (metodoPasarela is null)
        {
            throw new InvalidOperationException(
                "No está configurado el método de pago Pasarela.");
        }


        var clase = reserva.Sesion.Clase;
        var ahora = DateTime.UtcNow;

        var nuevoPago = new Pago
        {
            IdReserva = reserva.IdReserva,
            IdEstadoPago = estadoPagoPendiente.IdEstadoPago,
            IdMetodoPago = metodoPasarela.IdMetodoPago,
            IdMoneda = clase.IdMoneda,
            ValorOriginal = clase.Precio,
            FechaInicioPago = ahora,
            FechaCreacion = ahora,
            Observacion =
                "Pago iniciado. Pendiente de procesamiento de la pasarela."
        };


        var pagoCreado = await _pagoRepository.CrearAsync(nuevoPago);

        var resultadoPasarela = await ProcesarPagoEnPasarelaAsync(
            pagoCreado,
            clase.Moneda.CodigoISO);

        var nombreEstadoPago = ObtenerNombreEstadoPago(
            resultadoPasarela.Estado);

        var idEstadoResultado = await ObtenerIdEstadoPagoAsync(
            nombreEstadoPago);

        pagoCreado.IdEstadoPago = idEstadoResultado;
        pagoCreado.IdTransaccionExterna =
            resultadoPasarela.IdTransaccionExterna;
        pagoCreado.Observacion = resultadoPasarela.Mensaje;

        if (resultadoPasarela.Estado is
            EstadoResultadoPasarela.Aprobado or
            EstadoResultadoPasarela.Rechazado)
        {
            pagoCreado.FechaPago = DateTime.UtcNow;
        }

        if (resultadoPasarela.Estado == EstadoResultadoPasarela.Aprobado)
        {
            await using var transaccion =
                await _context.Database.BeginTransactionAsync();

            try
            {
                await _pagoRepository.ActualizarSinGuardarAsync(pagoCreado);

                await _reservaRepository.ConfirmarReservaPagadaAsync(
                    pagoCreado.IdReserva);

                await _context.SaveChangesAsync();

                await transaccion.CommitAsync();
            }
            catch
            {
                await transaccion.RollbackAsync();
                throw;
            }
        }
        else
        {
            await _pagoRepository.ActualizarAsync(pagoCreado);
        }

        return pagoCreado;



    }


    private async Task<long> ObtenerIdEstadoPagoAsync(string nombreEstado)
    {
        var estado = await _context.EstadosPago
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Nombre == nombreEstado);

        if (estado is null)
        {
            throw new InvalidOperationException(
                $"No está configurado el estado de pago '{nombreEstado}'.");
        }

        return estado.IdEstadoPago;
    }

    /// <summary>
    ///Preparar el procesamiento de la respuesta de la pasarela
    ///Este método deja los pagos con resultado técnico Error en estado Pendiente,
    /// </summary>
    private static string ObtenerNombreEstadoPago(
        EstadoResultadoPasarela resultado)
    {
        return resultado switch
        {
            EstadoResultadoPasarela.Pendiente => "Pendiente",
            EstadoResultadoPasarela.Aprobado => "Aprobado",
            EstadoResultadoPasarela.Rechazado => "Rechazado",
            EstadoResultadoPasarela.Error => "Pendiente",

            _ => throw new ArgumentOutOfRangeException(
                nameof(resultado),
                resultado,
                "La pasarela devolvió un resultado desconocido.")
        };
    }


    /// <summary>
    ///Método provisional Conectar la pasarela con IniciarPagoAsync
    /// </summary>

    private async Task<ResultadoPasarelaPago> ProcesarPagoEnPasarelaAsync(
        Pago pago,
        string codigoMoneda,
        CancellationToken cancellationToken = default)
    {
        var solicitud = new SolicitudPasarelaPago(
            pago.IdPago,
            pago.ValorOriginal,
            codigoMoneda,
            $"RESERVA-{pago.IdReserva}");

        return await _pasarelaPagoService.ProcesarPagoAsync(
            solicitud,
            cancellationToken);
    }


}

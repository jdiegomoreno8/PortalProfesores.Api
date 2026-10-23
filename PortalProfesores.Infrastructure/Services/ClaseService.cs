using PortalProfesores.Domain.DTOs.Clases;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Domain.Interfaces.Services;
using PortalProfesores.Domain.Entities;
using Azure.Core;
using PortalProfesores.Infrastructure.Repositories;

namespace PortalProfesores.Infrastructure.Services.Clases;

/// <summary>
/// Servicio encargado de consultar las clases.
/// </summary>
public class ClaseService : IClaseService
{
    private readonly IClaseRepository _claseRepository;
    private readonly INivelRepository _nivelRepository;
    private readonly IIdiomaRepository _idiomaRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IMonedaRepository _monedaRepository;
    private readonly IConfiguracionRepository _configuracionRepository;

    public ClaseService(
        IClaseRepository claseRepository,
    INivelRepository nivelRepository,
    IIdiomaRepository idiomaRepository,
    ICategoriaRepository categoriaRepository,
    IMonedaRepository monedaRepository,
    IConfiguracionRepository configuracionRepository)
    {
        _claseRepository = claseRepository;
        _nivelRepository = nivelRepository;
        _idiomaRepository = idiomaRepository;
        _categoriaRepository = categoriaRepository;
        _monedaRepository = monedaRepository;
        _configuracionRepository = configuracionRepository;
    }

    /// <summary>
    /// Obtiene el listado de clases.
    /// </summary>
    public async Task<IEnumerable<ClaseListadoDto>> ObtenerListadoAsync()
    {
        var clases =
            await _claseRepository.ObtenerListadoAsync();

        return clases.Select(x => new ClaseListadoDto
        {
            IdClase = x.IdClase,
            Titulo = x.Titulo,
            Descripcion = x.Descripcion,
            IdEstadoClase = x.IdEstadoClase,
            EstadoClase = x.EstadoClase?.Nombre ?? string.Empty,
            Idioma = x.Idioma?.Nombre ?? string.Empty,
            Nivel = x.Nivel?.Nombre ?? string.Empty,
            Categoria = x.Categoria?.Nombre ?? string.Empty,
            DuracionMinutos = x.DuracionMinutos,

            Precio = x.Precio,
            IdMoneda = x.IdMoneda,
            Moneda = x.Moneda?.Nombre ?? string.Empty,
            CodigoMoneda = x.Moneda?.CodigoISO ?? string.Empty,
            Orden = x.Orden
        });
    }

    /// <summary>
    /// Obtiene el detalle de una clase.
    /// </summary>
    public async Task<ClaseDetalleDto?> ObtenerPorIdAsync(
        long idClase)
    {
        if (idClase <= 0)
        {
            throw new ArgumentException(
                "El identificador de la clase no es válido.");
        }




        var clase =
            await _claseRepository.ObtenerPorIdAsync(idClase);

        if (clase is null)
        {
            return null;
        }

        return new ClaseDetalleDto
        {
            IdClase = clase.IdClase,

            IdIdioma = clase.IdIdioma,
            Idioma = clase.Idioma?.Nombre ?? string.Empty,

            IdNivel = clase.IdNivel,
            Nivel = clase.Nivel?.Nombre ?? string.Empty,

            IdCategoria = clase.IdCategoria,
            Categoria = clase.Categoria?.Nombre ?? string.Empty,

            IdEstadoClase = clase.IdEstadoClase,
            EstadoClase = clase.EstadoClase?.Nombre ?? string.Empty,


            Titulo = clase.Titulo,
            Descripcion = clase.Descripcion,
            UrlVideo = clase.UrlVideo,

            DuracionMinutos = clase.DuracionMinutos,


            Precio = clase.Precio,
            IdMoneda = clase.IdMoneda,
            Moneda = clase.Moneda?.Nombre ?? string.Empty,
            CodigoMoneda = clase.Moneda?.CodigoISO ?? string.Empty,

            Orden = clase.Orden,

            FechaCreacion = clase.FechaCreacion,
            FechaActualizacion = clase.FechaActualizacion
        };
    }
    private async Task<int> ObtenerDuracionClaseAsync()
    {
        var configuracion =
            await _configuracionRepository.ObtenerPorClaveAsync(
                "DURACION_CLASE");

        if (configuracion is null)
        {
            throw new InvalidOperationException(
                "No existe la configuración DURACION_CLASE.");
        }

        if (!int.TryParse(configuracion.Valor, out var duracion) ||
            duracion <= 0)
        {
            throw new InvalidOperationException(
                "La configuración DURACION_CLASE debe contener " +
                "un número entero mayor que cero.");
        }

        return duracion;
    }


    /// <summary>
    /// Crea una nueva clase.
    /// </summary>
    public async Task CrearAsync(CrearClaseRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Titulo))
        {
            throw new ArgumentException(
                "El título de la clase es obligatorio.");
        }

        if (request.IdIdioma <= 0)
        {
            throw new ArgumentException(
                "El idioma seleccionado no es válido.");
        }

        if (request.IdNivel <= 0)
        {
            throw new ArgumentException(
                "El nivel seleccionado no es válido.");
        }

        if (request.IdCategoria <= 0)
        {
            throw new ArgumentException(
                "La categoría seleccionada no es válida.");
        }

       


        if (request.Precio <= 0)
        {
            throw new ArgumentException(
                "El precio de la clase debe ser mayor que cero.");
        }

        if (request.IdMoneda <= 0)
        {
            throw new ArgumentException(
                "La moneda seleccionada no es válida.");
        }


        // Validar que el idioma exista.
        var idiomas = await _idiomaRepository.ObtenerTodosAsync();

        if (!idiomas.Any(x => x.IdIdioma == request.IdIdioma))
        {
            throw new ArgumentException(
                "El idioma seleccionado no existe.");
        }

        // Validar que el nivel exista.
        var niveles = await _nivelRepository.ObtenerTodosAsync();

        if (!niveles.Any(x => x.IdNivel == request.IdNivel))
        {
            throw new ArgumentException(
                "El nivel seleccionado no existe.");
        }

        // Validar que la categoría exista.
        var categorias = await _categoriaRepository.ObtenerTodosAsync();

        if (!categorias.Any(x => x.IdCategoria == request.IdCategoria))
        {
            throw new ArgumentException(
                "La categoría seleccionada no existe.");
        }


        var monedas = await _monedaRepository.ObtenerTodosAsync();

        if (!monedas.Any(x => x.IdMoneda == request.IdMoneda))
        {
            throw new ArgumentException(
                "La moneda seleccionada no existe.");
        }


        // Obtener el siguiente orden dentro de la combinación
        // Idioma + Nivel + Categoría.
        var siguienteOrden =
            await _claseRepository.ObtenerSiguienteOrdenAsync(
                request.IdIdioma,
                request.IdNivel,
                request.IdCategoria);

        var duracion = await ObtenerDuracionClaseAsync();

        var clase = new Clase
        {
            IdIdioma = request.IdIdioma,
            IdNivel = request.IdNivel,
            IdCategoria = request.IdCategoria,

            Titulo = request.Titulo.Trim(),
            Descripcion = request.Descripcion?.Trim(),
            UrlVideo = request.UrlVideo?.Trim(),
            DuracionMinutos = duracion,

            Orden = siguienteOrden,

            Precio = request.Precio,
            IdMoneda = request.IdMoneda,

            // Toda clase nueva inicia activa.
            IdEstadoClase = 1,

            FechaCreacion = DateTime.UtcNow
        };

        await _claseRepository.CrearAsync(clase);
    }

    /// <summary>
    /// Suspende una clase existente.
    /// </summary>
    public async Task SuspenderAsync(long idClase)
    {
        if (idClase <= 0)
        {
            throw new ArgumentException(
                "El identificador de la clase no es válido.");
        }

        var clase =
            await _claseRepository.ObtenerPorIdAsync(idClase);

        if (clase is null)
        {
            throw new KeyNotFoundException(
                "La clase no fue encontrada.");
        }

        if (clase.IdEstadoClase == 2)
        {
            throw new InvalidOperationException(
                "La clase ya se encuentra suspendida.");
        }

        var resultado =
            await _claseRepository.SuspenderAsync(idClase);

        if (!resultado)
        {
            throw new KeyNotFoundException(
                "La clase no fue encontrada.");
        }
    }

    /// <summary>
    /// Activa nuevamente una clase suspendida.
    /// </summary>
    public async Task ActivarAsync(long idClase)
    {
        if (idClase <= 0)
        {
            throw new ArgumentException(
                "El identificador de la clase no es válido.");
        }

        var clase =
            await _claseRepository.ObtenerPorIdAsync(idClase);

        if (clase is null)
        {
            throw new KeyNotFoundException(
                "La clase no fue encontrada.");
        }

        if (clase.IdEstadoClase == 1)
        {
            throw new InvalidOperationException(
                "La clase ya se encuentra activa.");
        }

        var resultado =
            await _claseRepository.ActivarAsync(idClase);

        if (!resultado)
        {
            throw new KeyNotFoundException(
                "La clase no fue encontrada.");
        }
    }

    /// <summary>
    /// Actualiza una clase existente.
    /// </summary>
    public async Task EditarAsync(
        long idClase,
        EditarClaseRequestDto request)
    {
        if (idClase <= 0)
        {
            throw new ArgumentException(
                "El identificador de la clase no es válido.");
        }

        if (string.IsNullOrWhiteSpace(request.Titulo))
        {
            throw new ArgumentException(
                "El título de la clase es obligatorio.");
        }

        if (request.IdIdioma <= 0)
        {
            throw new ArgumentException(
                "El idioma seleccionado no es válido.");
        }

        if (request.IdNivel <= 0)
        {
            throw new ArgumentException(
                "El nivel seleccionado no es válido.");
        }

        if (request.IdCategoria <= 0)
        {
            throw new ArgumentException(
                "La categoría seleccionada no es válida.");
        }

       

        if (request.Precio <= 0)
        {
            throw new ArgumentException(
                "El precio de la clase debe ser mayor que cero.");
        }

        if (request.IdMoneda <= 0)
        {
            throw new ArgumentException(
                "La moneda seleccionada no es válida.");
        }

        var monedas = await _monedaRepository.ObtenerTodosAsync();

        if (!monedas.Any(x => x.IdMoneda == request.IdMoneda))
        {
            throw new ArgumentException(
                "La moneda seleccionada no existe.");
        }



        var idiomas = await _idiomaRepository.ObtenerTodosAsync();

        if (!idiomas.Any(x => x.IdIdioma == request.IdIdioma))
        {
            throw new ArgumentException(
                "El idioma seleccionado no existe.");
        }

        var niveles = await _nivelRepository.ObtenerTodosAsync();

        if (!niveles.Any(x => x.IdNivel == request.IdNivel))
        {
            throw new ArgumentException(
                "El nivel seleccionado no existe.");
        }

        var categorias = await _categoriaRepository.ObtenerTodosAsync();

        if (!categorias.Any(x => x.IdCategoria == request.IdCategoria))
        {
            throw new ArgumentException(
                "La categoría seleccionada no existe.");
        }
        var duracion = await ObtenerDuracionClaseAsync();

        var actualizado = await _claseRepository.EditarAsync(
     idClase,
     request,
     duracion);

        if (!actualizado)
        {
            throw new KeyNotFoundException(
                "La clase no fue encontrada.");
        }
    }


}
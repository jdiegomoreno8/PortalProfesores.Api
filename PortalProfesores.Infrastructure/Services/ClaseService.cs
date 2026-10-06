using PortalProfesores.Domain.DTOs.Clases;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Domain.Interfaces.Services;
using PortalProfesores.Domain.Entities;

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

    public ClaseService(
        IClaseRepository claseRepository,
    INivelRepository nivelRepository,
    IIdiomaRepository idiomaRepository,
    ICategoriaRepository categoriaRepository)
    {
        _claseRepository = claseRepository;
        _nivelRepository = nivelRepository;
        _idiomaRepository = idiomaRepository;
        _categoriaRepository = categoriaRepository;
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
            Orden = clase.Orden,

            FechaCreacion = clase.FechaCreacion,
            FechaActualizacion = clase.FechaActualizacion
        };
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

        if (request.DuracionMinutos.HasValue &&
            request.DuracionMinutos <= 0)
        {
            throw new ArgumentException(
                "La duración de la clase no es válida.");
        }

        if (request.Orden <= 0)
        {
            throw new ArgumentException(
                "El orden de la clase no es válido.");
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

        var clase = new Clase
        {
            IdIdioma = request.IdIdioma,
            IdNivel = request.IdNivel,
            IdCategoria = request.IdCategoria,

            Titulo = request.Titulo.Trim(),
            Descripcion = request.Descripcion?.Trim(),
            UrlVideo = request.UrlVideo?.Trim(),

            DuracionMinutos = request.DuracionMinutos,
            Orden = request.Orden,

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

        if (request.DuracionMinutos.HasValue &&
            request.DuracionMinutos <= 0)
        {
            throw new ArgumentException(
                "La duración de la clase no es válida.");
        }

        if (request.Orden <= 0)
        {
            throw new ArgumentException(
                "El orden de la clase no es válido.");
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

        var actualizado = await _claseRepository.EditarAsync(
            idClase,
            request);

        if (!actualizado)
        {
            throw new KeyNotFoundException(
                "La clase no fue encontrada.");
        }
    }
}
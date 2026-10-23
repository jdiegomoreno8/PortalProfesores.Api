using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Hosting;
using PortalProfesores.Domain.Exceptions;
using PortalProfesores.Domain.Interfaces.Services;

namespace PortalProfesores.Infrastructure.Services;

public class FotoPerfilStorageService : IFotoPerfilStorageService
{
    private const int TamanoMaximoBytes = 5 * 1024 * 1024;

    private static readonly byte[] FirmaPng =
    [
        0x89, 0x50, 0x4E, 0x47,
        0x0D, 0x0A, 0x1A, 0x0A
    ];

    private readonly string _directorioUploads;

    public FotoPerfilStorageService(IHostEnvironment environment)
    {
        _directorioUploads = Path.Combine(
            environment.ContentRootPath,
            "wwwroot",
            "uploads");
    }

    public async Task<string> GuardarAsync(
        Stream archivo,
        string nombrePerfil,
        string tipoPerfil)
    {
        ArgumentNullException.ThrowIfNull(archivo);

        var carpeta = ObtenerCarpeta(tipoPerfil);

        if (!archivo.CanRead)
        {
            throw new InvalidOperationException(
                "No es posible leer el archivo recibido.");
        }

        if (string.IsNullOrWhiteSpace(nombrePerfil))
        {
            throw new ArgumentException(
                "El nombre del perfil es obligatorio.",
                nameof(nombrePerfil));
        }

        using var memoria = new MemoryStream();
        var buffer = new byte[81920];
        int bytesLeidos;
        long totalBytes = 0;

        while ((bytesLeidos = await archivo.ReadAsync(buffer)) > 0)
        {
            totalBytes += bytesLeidos;

            if (totalBytes > TamanoMaximoBytes)
            {
                throw new ValidationException(
                    "La fotografía no puede superar los 5 MB.");
            }

            await memoria.WriteAsync(
                buffer.AsMemory(0, bytesLeidos));
        }

        if (memoria.Length < FirmaPng.Length)
        {
            throw new ValidationException(
                "El archivo no es un PNG válido.");
        }

        var contenido = memoria.ToArray();

        if (!contenido.AsSpan(0, FirmaPng.Length)
            .SequenceEqual(FirmaPng))
        {
            throw new ValidationException(
                "Solo se permiten imágenes PNG.");
        }

        var nombreSeguro = NormalizarNombre(nombrePerfil);
        var identificador = Guid.NewGuid().ToString("N")[..8];
        var nombreArchivo = $"{nombreSeguro}_{identificador}.png";

        var directorioFotos = Path.Combine(
            _directorioUploads,
            carpeta);

        Directory.CreateDirectory(directorioFotos);

        var rutaCompleta = Path.Combine(
            directorioFotos,
            nombreArchivo);

        await File.WriteAllBytesAsync(rutaCompleta, contenido);

        return $"/uploads/{carpeta}/{nombreArchivo}";
    }

    public Task EliminarAsync(
        string? rutaFoto,
        string tipoPerfil)
    {
        if (string.IsNullOrWhiteSpace(rutaFoto))
        {
            return Task.CompletedTask;
        }

        var carpeta = ObtenerCarpeta(tipoPerfil);
        var prefijo = $"/uploads/{carpeta}/";

        if (!rutaFoto.StartsWith(
            prefijo,
            StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException(
                "La ruta de la fotografía no es válida.",
                nameof(rutaFoto));
        }

        var nombreArchivo = rutaFoto[prefijo.Length..];

        if (string.IsNullOrWhiteSpace(nombreArchivo) ||
            nombreArchivo != Path.GetFileName(nombreArchivo) ||
            !nombreArchivo.EndsWith(
                ".png",
                StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException(
                "La ruta de la fotografía no es válida.",
                nameof(rutaFoto));
        }

        var rutaCompleta = Path.Combine(
            _directorioUploads,
            carpeta,
            nombreArchivo);

        if (File.Exists(rutaCompleta))
        {
            File.Delete(rutaCompleta);
        }

        return Task.CompletedTask;
    }

    private static string ObtenerCarpeta(string tipoPerfil)
    {
        return tipoPerfil.Trim().ToLowerInvariant() switch
        {
            "profesores" => "profesores",
            "estudiantes" => "estudiantes",
            _ => throw new ArgumentException(
                "El tipo de perfil no es válido.",
                nameof(tipoPerfil))
        };
    }

    private static string NormalizarNombre(string nombre)
    {
        var normalizado = nombre.Trim().ToLowerInvariant()
            .Normalize(NormalizationForm.FormD);

        var sinTildes = new string(
            normalizado
                .Where(c =>
                    System.Globalization.CharUnicodeInfo
                        .GetUnicodeCategory(c) !=
                    System.Globalization.UnicodeCategory.NonSpacingMark)
                .ToArray());

        var resultado = Regex.Replace(
            sinTildes,
            @"[^a-z0-9]+",
            "_").Trim('_');

        return string.IsNullOrWhiteSpace(resultado)
            ? "perfil"
            : resultado;
    }
}
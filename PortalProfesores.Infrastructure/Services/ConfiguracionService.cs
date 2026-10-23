
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Domain.Interfaces.Services;

namespace PortalProfesores.Infrastructure.Services;

public class ConfiguracionService : IConfiguracionService
{
    private readonly IConfiguracionRepository _configuracionRepository;

    public ConfiguracionService(
        IConfiguracionRepository configuracionRepository)
    {
        _configuracionRepository = configuracionRepository;
    }

    public async Task<int> ObtenerEnteroPositivoAsync(string clave)
    {
        if (string.IsNullOrWhiteSpace(clave))
        {
            throw new ArgumentException(
                "La clave de configuración es obligatoria.",
                nameof(clave));
        }

        var configuracion =
            await _configuracionRepository.ObtenerPorClaveAsync(clave);

        if (configuracion is null)
        {
            throw new InvalidOperationException(
                $"No existe la configuración '{clave}'.");
        }

        if (!string.Equals(
                configuracion.TipoDato,
                "INT",
                StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"La configuración '{clave}' no es de tipo INT.");
        }

        if (!int.TryParse(configuracion.Valor, out var valor) || valor <= 0)
        {
            throw new InvalidOperationException(
                $"La configuración '{clave}' debe contener un entero positivo.");
        }

        return valor;
    }
}

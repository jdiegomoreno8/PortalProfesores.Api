namespace PortalProfesores.Domain.Interfaces.Services;

public interface IFotoPerfilStorageService
{
    Task<string> GuardarAsync(
        Stream archivo,
        string nombrePerfil,
        string tipoPerfil);

    Task EliminarAsync(
        string? rutaFoto,
        string tipoPerfil);
}
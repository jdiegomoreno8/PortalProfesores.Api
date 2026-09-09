namespace PortalProfesores.Domain.Exceptions;

/// <summary>
/// Recurso no encontrado.
/// </summary>
public class NotFoundException : BaseException
{
    public NotFoundException(string mensaje)
        : base(mensaje)
    {
    }
}
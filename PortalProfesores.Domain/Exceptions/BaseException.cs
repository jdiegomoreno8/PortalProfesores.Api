namespace PortalProfesores.Domain.Exceptions;

/// <summary>
/// Excepción base de la aplicación.
/// </summary>
public abstract class BaseException : Exception
{
    protected BaseException(string mensaje)
        : base(mensaje)
    {
    }
}
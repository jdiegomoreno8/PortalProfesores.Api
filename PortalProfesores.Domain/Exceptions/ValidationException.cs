namespace PortalProfesores.Domain.Exceptions;

/// <summary>
/// Error de validación.
/// </summary>
public class ValidationException : BaseException
{
    public ValidationException(string mensaje)
        : base(mensaje)
    {
    }
}
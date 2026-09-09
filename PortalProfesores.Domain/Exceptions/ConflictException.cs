namespace PortalProfesores.Domain.Exceptions;

/// <summary>
/// Conflicto de información.
/// </summary>
public class ConflictException : BaseException
{
    public ConflictException(string mensaje)
        : base(mensaje)
    {
    }
}
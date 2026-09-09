namespace PortalProfesores.Domain.Exceptions;

/// <summary>
/// Usuario no autorizado.
/// </summary>
public class UnauthorizedException : BaseException
{
    public UnauthorizedException(string mensaje)
        : base(mensaje)
    {
    }
}
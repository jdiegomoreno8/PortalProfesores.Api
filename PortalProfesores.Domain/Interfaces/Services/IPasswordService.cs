namespace PortalProfesores.Domain.Interfaces.Services;

/// <summary>
/// Define las operaciones relacionadas con
/// el manejo seguro de contraseñas.
/// </summary>
public interface IPasswordService
{
    string HashPassword(string password);

    bool VerifyPassword(string password, string hash);
}
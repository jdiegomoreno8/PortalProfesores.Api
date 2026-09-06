using PortalProfesores.Domain.DTOs.Auth;

namespace PortalProfesores.Domain.Interfaces.Services;

/// <summary>
/// Define las operaciones relacionadas con la autenticación.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Inicia el proceso de autenticación de un usuario.
    /// </summary>
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    Task<RegisterResponseDto> RegistrarAsync(RegisterRequestDto request);
}
using Microsoft.AspNetCore.Mvc;
using PortalProfesores.Domain.DTOs.Auth;
using PortalProfesores.Domain.Interfaces.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace PortalProfesores.Api.Controllers;

/// <summary>
/// Expone los endpoints relacionados con la autenticación.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Inicia el proceso de autenticación de un usuario.
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var response = await _authService.LoginAsync(request);

        return Ok(response);
    }

    /// <summary>
    /// Registra un nuevo usuario en la plataforma.
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto request)
    {
        var response = await _authService.RegistrarAsync(request);

        return Ok(response);
    }

  

}
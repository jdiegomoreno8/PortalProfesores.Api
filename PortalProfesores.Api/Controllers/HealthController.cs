using Microsoft.AspNetCore.Mvc;
using PortalProfesores.Domain.Interfaces.Services;
using PortalProfesores.Shared.Responses;


namespace PortalProfesores.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly IHealthService _healthService;

    public HealthController(IHealthService healthService)
    {
        _healthService = healthService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            bool conexion = await _healthService.CanConnectAsync();

            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Message = "Conexión realizada correctamente.",
                Data = conexion
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Ocurrió un error.",
                Data = ex.Message
            });
        }
    }
}
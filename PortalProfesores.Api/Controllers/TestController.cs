using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PortalProfesores.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("publico")]
    public IActionResult Publico()
    {
        return Ok("Endpoint público.");
    }

    [Authorize]
    [HttpGet("privado")]
    public IActionResult Privado()
    {
        return Ok("Acceso autorizado.");
    }
}
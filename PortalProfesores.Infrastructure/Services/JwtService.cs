using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PortalProfesores.Domain.Interfaces.Services;
using PortalProfesores.Infrastructure.Configuration;

namespace PortalProfesores.Infrastructure.Services;

/// <summary>
/// Servicio encargado de generar tokens JWT.
/// </summary>
public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;

    public JwtService(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerarToken(long idUsuario,
                               string correo,
                               string nombreCompleto)
    {
        var claims = new List<Claim>
        {
               new Claim(ClaimTypes.NameIdentifier, idUsuario.ToString()),
               new Claim(JwtRegisteredClaimNames.Sub, idUsuario.ToString()),
               new Claim(JwtRegisteredClaimNames.Email, correo),
               new Claim(JwtRegisteredClaimNames.Name, nombreCompleto),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Key));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
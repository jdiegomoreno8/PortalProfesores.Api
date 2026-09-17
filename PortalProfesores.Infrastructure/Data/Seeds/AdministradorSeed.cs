using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PortalProfesores.Domain.Entities;
using PortalProfesores.Domain.Interfaces.Services;

namespace PortalProfesores.Infrastructure.Data.Seeds;

/// <summary>
/// Inicializa el administrador principal del sistema
/// cuando todavía no existe ningún administrador.
/// </summary>
public static class AdministradorSeed
{
    private const long RolAdministrador = 1;
    private const long EstadoUsuarioActivo = 1;

    public static async Task InicializarAsync(
        PortalProfesoresDbContext context,
        IPasswordService passwordService,
        IConfiguration configuration)
    {
        // Verificamos si ya existe un administrador.
        var existeAdministrador = await context.UsuariosRoles
            .AnyAsync(x => x.IdRol == RolAdministrador);

        if (existeAdministrador)
        {
            return;
        }

        var configuracion = configuration
            .GetSection("AdministradorInicial");

        var nombres = configuracion["Nombres"];
        var apellidos = configuracion["Apellidos"];
        var idTipoDocumentoTexto = configuracion["IdTipoDocumento"];
        var numeroDocumento = configuracion["NumeroDocumento"];
        var correo = configuracion["CorreoElectronico"];
        var clave = configuracion["Clave"];

        if (string.IsNullOrWhiteSpace(nombres)
            || string.IsNullOrWhiteSpace(apellidos)
            || string.IsNullOrWhiteSpace(idTipoDocumentoTexto)
            || string.IsNullOrWhiteSpace(numeroDocumento)
            || string.IsNullOrWhiteSpace(correo)
            || string.IsNullOrWhiteSpace(clave))
        {
            throw new InvalidOperationException(
                "La configuración del administrador inicial no está completa.");
        }

        if (!long.TryParse(
                idTipoDocumentoTexto,
                out var idTipoDocumento))
        {
            throw new InvalidOperationException(
                "El IdTipoDocumento del administrador inicial no es válido.");
        }

        var existeCorreo = await context.Personas
            .AnyAsync(x => x.CorreoElectronico == correo);

        if (existeCorreo)
        {
            throw new InvalidOperationException(
                $"Ya existe una persona registrada con el correo {correo}.");
        }

        var ahora = DateTime.UtcNow;

        var persona = new Persona
        {
            IdTipoDocumento = idTipoDocumento,
            NumeroDocumento = numeroDocumento.Trim(),
            Nombres = nombres.Trim(),
            Apellidos = apellidos.Trim(),
            CorreoElectronico = correo.Trim(),
            FechaIngreso = ahora
        };

        var usuario = new Usuario
        {
            IdEstadoUsuario = EstadoUsuarioActivo,
            ClaveHash = passwordService.HashPassword(clave),
            Persona = persona
        };

        usuario.Roles.Add(new UsuarioRol
        {
            IdRol = RolAdministrador,
            Usuario = usuario
        });

        context.Personas.Add(persona);
        context.Usuarios.Add(usuario);

        await context.SaveChangesAsync();
    }
}
using Microsoft.Extensions.DependencyInjection;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Domain.Interfaces.Services;
using PortalProfesores.Infrastructure.Repositories;
using PortalProfesores.Infrastructure.Services;
using PortalProfesores.Infrastructure.Services.Catalogos;

using PortalProfesores.Infrastructure.Services.Clases;

namespace PortalProfesores.Infrastructure.DependencyInjection;

/// <summary>
/// Centraliza el registro de las dependencias utilizadas por la aplicación.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        #region Repositorios

        services.AddScoped<IHealthRepository, HealthRepository>();

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        services.AddScoped<IProfesorDetalleRepository, ProfesorDetalleRepository>();    
				
		services.AddScoped<IPersonaRepository, PersonaRepository>();

        services.AddScoped<ITipoDocumentoRepository, TipoDocumentoRepository>();

        services.AddScoped<ICiudadRepository, CiudadRepository>();

        services.AddScoped<INivelRepository, NivelRepository>();

        services.AddScoped<IIdiomaRepository, IdiomaRepository>();

        services.AddScoped<ICategoriaRepository, CategoriaRepository>();

        services.AddScoped<IProfesorRepository, ProfesorRepository>();

        // Repositorio encargado del acceso a los datos de los estudiantes.
        services.AddScoped<IEstudianteRepository, EstudianteRepository>();

        services.AddScoped<IClaseRepository, ClaseRepository>();

       






        #endregion

        #region Servicios

        services.AddScoped<IHealthService, HealthService>();

        services.AddScoped<IUsuarioService, UsuarioService>();

        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IJwtService, JwtService>();

        services.AddScoped<IPasswordService, PasswordService>();

        services.AddScoped<IProfesorService, ProfesorService>();

        services.AddScoped<IAdministradorService, AdministradorService>();      

        services.AddScoped<ICatalogoService, CatalogoService>();

        services.AddScoped<IEstudianteService, EstudianteService>();

        services.AddScoped<IClaseService, ClaseService>();

        #endregion

        return services;
    }
}
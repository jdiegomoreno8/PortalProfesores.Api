using Microsoft.EntityFrameworkCore;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data;

/// <summary>
/// Contexto principal de Entity Framework.
/// Centraliza el acceso a la base de datos y registra
/// las entidades utilizadas por la aplicación.
/// </summary>
public class PortalProfesoresDbContext : DbContext
{
    public PortalProfesoresDbContext(DbContextOptions<PortalProfesoresDbContext> options)
        : base(options)
    {
    }
    #region DbSets

    public DbSet<Persona> Personas => Set<Persona>();

    public DbSet<Usuario> Usuarios => Set<Usuario>();

    public DbSet<EstadoUsuario> EstadosUsuario => Set<EstadoUsuario>();

    public DbSet<Rol> Roles => Set<Rol>();

    public DbSet<UsuarioRol> UsuariosRoles => Set<UsuarioRol>();

    public DbSet<TipoDocumento> TiposDocumento => Set<TipoDocumento>();

    public DbSet<Ciudad> Ciudades => Set<Ciudad>();
    public DbSet<ProfesorDetalle> ProfesoresDetalle => Set<ProfesorDetalle>();

    // Catálogo de niveles académicos.
    // Es utilizado actualmente por EstudianteDetalle
    // y posteriormente será utilizado por Microclase.
    public DbSet<Nivel> Niveles => Set<Nivel>();

    public DbSet<Idioma> Idiomas => Set<Idioma>();

    public DbSet<Categoria> Categorias => Set<Categoria>();

    // Clases disponibles en el portal.
    public DbSet<Clase> Clases => Set<Clase>();

    public DbSet<Sesion> Sesiones => Set<Sesion>();
    public DbSet<EstudianteDetalle> EstudiantesDetalle => Set<EstudianteDetalle>();

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Registra automáticamente todas las configuraciones
        // IEntityTypeConfiguration<T> del proyecto Infrastructure.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PortalProfesoresDbContext).Assembly);
    }

   

    /// <summary>
    /// Persiste en la base de datos todas las entidades que
    /// se encuentran pendientes en el DbContext.
    /// </summary>
    public async Task GuardarCambiosAsync()
    {
        await SaveChangesAsync();
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración de Entity Framework para Clase.
/// </summary>
public class ClaseConfiguration
    : IEntityTypeConfiguration<Clase>
{
    public void Configure(EntityTypeBuilder<Clase> builder)
    {
        builder.ToTable("Clase");

        // Clave primaria
        builder.HasKey(x => x.IdClase);

        builder.Property(x => x.IdClase)
               .HasColumnName("IdClase");

        // Idioma
        builder.Property(x => x.IdIdioma)
               .HasColumnName("IdIdioma")
               .IsRequired();

        builder.HasOne(x => x.Idioma)
               .WithMany(x => x.Clases)
               .HasForeignKey(x => x.IdIdioma)
               .OnDelete(DeleteBehavior.Restrict);

        // Nivel
        builder.Property(x => x.IdNivel)
               .HasColumnName("IdNivel")
               .IsRequired();

        builder.HasOne(x => x.Nivel)
               .WithMany()
               .HasForeignKey(x => x.IdNivel)
               .OnDelete(DeleteBehavior.Restrict);

        // Categoría
        builder.Property(x => x.IdCategoria)
               .HasColumnName("IdCategoria")
               .IsRequired();

        builder.HasOne(x => x.Categoria)
               .WithMany(x => x.Clases)
               .HasForeignKey(x => x.IdCategoria)
               .OnDelete(DeleteBehavior.Restrict);

        // ---------------------------------------------------------
        // ESTADO DE LA CLASE
        // ---------------------------------------------------------

        builder.Property(x => x.IdEstadoClase)
               .HasColumnName("IdEstadoClase")
               .IsRequired();

        builder.HasOne(x => x.EstadoClase)
               .WithMany(x => x.Clases)
               .HasForeignKey(x => x.IdEstadoClase)
               .OnDelete(DeleteBehavior.Restrict);

        // Información de la clase
        builder.Property(x => x.Titulo)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(x => x.Descripcion)
               .HasColumnType("nvarchar(max)");

        builder.Property(x => x.UrlVideo)
               .HasMaxLength(500);

        builder.Property(x => x.DuracionMinutos);

        builder.Property(x => x.Orden)
               .IsRequired();

        // Relación Clase -> Sesiones
        builder.HasMany(x => x.Sesiones)
               .WithOne(x => x.Clase)
               .HasForeignKey(x => x.IdClase)
               .OnDelete(DeleteBehavior.Restrict);

        // Auditoría
        builder.Property(x => x.FechaCreacion)
               .IsRequired();

        builder.Property(x => x.UsuarioCreacion);

        builder.Property(x => x.FechaActualizacion);

        builder.Property(x => x.UsuarioActualizacion);
    }
}
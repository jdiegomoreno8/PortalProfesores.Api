using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración de Entity Framework para EstadoClase.
/// </summary>
public class EstadoClaseConfiguration
    : IEntityTypeConfiguration<EstadoClase>
{
    public void Configure(EntityTypeBuilder<EstadoClase> builder)
    {
        builder.ToTable("EstadoClase");

        // Clave primaria
        builder.HasKey(x => x.IdEstadoClase);

        builder.Property(x => x.IdEstadoClase)
               .HasColumnName("IdEstadoClase");

        // Nombre
        builder.Property(x => x.Nombre)
               .HasColumnName("Nombre")
               .HasMaxLength(50)
               .IsRequired();

        // Descripción
        builder.Property(x => x.Descripcion)
               .HasColumnName("Descripcion")
               .HasMaxLength(250);

        // Relación con Clase
        builder.HasMany(x => x.Clases)
               .WithOne(x => x.EstadoClase)
               .HasForeignKey(x => x.IdEstadoClase)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
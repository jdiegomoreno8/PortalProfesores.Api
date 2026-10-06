using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración de Entity Framework para Idioma.
/// </summary>
public class IdiomaConfiguration
    : IEntityTypeConfiguration<Idioma>
{
    public void Configure(EntityTypeBuilder<Idioma> builder)
    {
        builder.ToTable("Idioma");

        builder.HasKey(x => x.IdIdioma);

        builder.Property(x => x.IdIdioma)
               .HasColumnName("IdIdioma");

        builder.Property(x => x.Nombre)
               .HasColumnName("Nombre")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.CodigoISO)
               .HasColumnName("CodigoISO")
               .HasMaxLength(10)
               .IsRequired();

        builder.Property(x => x.FechaCreacion)
               .HasColumnName("FechaCreacion")
               .IsRequired();

        builder.Property(x => x.UsuarioCreacion)
               .HasColumnName("UsuarioCreacion");

        builder.Property(x => x.FechaActualizacion)
               .HasColumnName("FechaActualizacion");

        builder.Property(x => x.UsuarioActualizacion)
               .HasColumnName("UsuarioActualizacion");
    }
}
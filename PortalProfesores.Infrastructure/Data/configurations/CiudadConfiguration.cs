using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración de la entidad Ciudad.
/// </summary>
public class CiudadConfiguration : IEntityTypeConfiguration<Ciudad>
{
    public void Configure(EntityTypeBuilder<Ciudad> builder)
    {
        builder.ToTable("Ciudad");

        builder.HasKey(x => x.IdCiudad);

        builder.Property(x => x.IdCiudad)
               .HasColumnName("IdCiudad");

        builder.Property(x => x.IdPais)
               .HasColumnName("IdPais")
               .IsRequired();

        builder.Property(x => x.Nombre)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(x => x.FechaCreacion)
               .IsRequired();

        builder.Property(x => x.UsuarioCreacion);

        builder.Property(x => x.FechaActualizacion);

        builder.Property(x => x.UsuarioActualizacion);
    }
}
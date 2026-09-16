using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración de la entidad TipoDocumento.
/// </summary>
public class TipoDocumentoConfiguration : IEntityTypeConfiguration<TipoDocumento>
{
    public void Configure(EntityTypeBuilder<TipoDocumento> builder)
    {
        builder.ToTable("TipoDocumento");

        builder.HasKey(x => x.IdTipoDocumento);

        builder.Property(x => x.IdTipoDocumento)
               .HasColumnName("IdTipoDocumento");

        builder.Property(x => x.Nombre)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(x => x.Abreviatura)
               .HasMaxLength(40)
               .IsRequired();

        builder.Property(x => x.Descripcion)
               .HasMaxLength(600);

        builder.Property(x => x.FechaCreacion)
               .IsRequired();

        builder.Property(x => x.UsuarioCreacion);

        builder.Property(x => x.FechaActualizacion);

        builder.Property(x => x.UsuarioActualizacion);
    }
}
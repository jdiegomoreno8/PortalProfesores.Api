using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración de la entidad Rol.
/// </summary>
public class RolConfiguration : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> builder)
    {
        builder.ToTable("Rol");

        builder.HasKey(x => x.IdRol);

        builder.Property(x => x.IdRol)
               .HasColumnName("IdRol");

        builder.Property(x => x.Nombre)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.Descripcion)
               .HasMaxLength(300);
    }
}
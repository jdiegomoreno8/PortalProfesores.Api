using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración de la entidad EstadoUsuario.
/// </summary>
public class EstadoUsuarioConfiguration : IEntityTypeConfiguration<EstadoUsuario>
{
    public void Configure(EntityTypeBuilder<EstadoUsuario> builder)
    {
        builder.ToTable("EstadoUsuario");

        builder.HasKey(x => x.IdEstadoUsuario);

        builder.Property(x => x.IdEstadoUsuario)
               .HasColumnName("IdEstadoUsuario");

        builder.Property(x => x.Nombre)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.Descripcion)
               .HasMaxLength(500);

        builder.HasMany(x => x.Usuarios)
               .WithOne(x => x.EstadoUsuario)
               .HasForeignKey(x => x.IdEstadoUsuario);
    }
}
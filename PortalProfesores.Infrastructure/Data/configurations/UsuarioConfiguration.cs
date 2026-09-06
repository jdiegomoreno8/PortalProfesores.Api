using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración de la entidad Usuario.
/// </summary>
public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuario");

        builder.HasKey(x => x.IdUsuario);

        builder.Property(x => x.IdUsuario)
               .HasColumnName("IdUsuario");

        builder.Property(x => x.IdPersona)
               .HasColumnName("IdPersona")
               .IsRequired();

        builder.Property(x => x.IdEstadoUsuario)
               .HasColumnName("IdEstadoUsuario")
               .IsRequired();

        builder.Property(x => x.NombreUsuario)
               .HasMaxLength(200);

        builder.Property(x => x.ClaveHash)
               .HasMaxLength(510);

        builder.Property(x => x.UltimoAcceso);

        builder.HasOne(x => x.Persona)
               .WithOne(x => x.Usuario)
               .HasForeignKey<Usuario>(x => x.IdPersona);

        builder.HasOne(x => x.EstadoUsuario)
               .WithMany(x => x.Usuarios)
               .HasForeignKey(x => x.IdEstadoUsuario);
    }
}
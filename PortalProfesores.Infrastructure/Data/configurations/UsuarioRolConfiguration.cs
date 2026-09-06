using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración de la relación entre Usuario y Rol.
/// </summary>
public class UsuarioRolConfiguration : IEntityTypeConfiguration<UsuarioRol>
{
    public void Configure(EntityTypeBuilder<UsuarioRol> builder)
    {
        builder.ToTable("UsuarioRol");

        // Llave primaria compuesta
        builder.HasKey(x => new
        {
            x.IdUsuario,
            x.IdRol
        });

        builder.Property(x => x.IdUsuario)
               .HasColumnName("IdUsuario");

        builder.Property(x => x.IdRol)
               .HasColumnName("IdRol");

        builder.HasOne(x => x.Usuario)
               .WithMany()
               .HasForeignKey(x => x.IdUsuario);

        builder.HasOne(x => x.Rol)
               .WithMany(x => x.Usuarios)
               .HasForeignKey(x => x.IdRol);
    }
}
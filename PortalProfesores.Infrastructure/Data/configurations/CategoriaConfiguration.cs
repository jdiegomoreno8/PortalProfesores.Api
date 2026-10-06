using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración de Entity Framework para Categoria.
/// </summary>
public class CategoriaConfiguration
    : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categoria");

        builder.HasKey(x => x.IdCategoria);

        builder.Property(x => x.IdCategoria)
               .HasColumnName("IdCategoria");

        builder.Property(x => x.Nombre)
               .HasColumnName("Nombre")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.Descripcion)
               .HasColumnName("Descripcion")
               .HasMaxLength(250);

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
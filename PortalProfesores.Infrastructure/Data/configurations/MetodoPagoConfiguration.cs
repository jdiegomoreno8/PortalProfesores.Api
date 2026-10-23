using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

public class MetodoPagoConfiguration
    : IEntityTypeConfiguration<MetodoPago>
{
    public void Configure(EntityTypeBuilder<MetodoPago> builder)
    {
        builder.ToTable("MetodoPago");

        builder.HasKey(e => e.IdMetodoPago);

        builder.Property(e => e.IdMetodoPago)
            .HasColumnName("IdMetodoPago");

        builder.Property(e => e.Nombre)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Descripcion)
            .HasMaxLength(500);

        builder.Property(e => e.FechaCreacion)
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(e => e.UsuarioCreacion);

        builder.Property(e => e.FechaActualizacion)
            .HasColumnType("datetime2");

        builder.Property(e => e.UsuarioActualizacion);
    }
}
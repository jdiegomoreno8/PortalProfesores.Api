using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

public class MonedaConfiguration
    : IEntityTypeConfiguration<Moneda>
{
    public void Configure(EntityTypeBuilder<Moneda> builder)
    {
        builder.ToTable("Moneda");

        builder.HasKey(m => m.IdMoneda);

        builder.Property(m => m.IdMoneda)
            .HasColumnName("IdMoneda");

        builder.Property(m => m.Nombre)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(m => m.CodigoISO)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(m => m.Simbolo)
            .HasMaxLength(10);

        builder.Property(m => m.FechaCreacion)
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(m => m.UsuarioCreacion);

        builder.Property(m => m.FechaActualizacion)
            .HasColumnType("datetime2");

        builder.Property(m => m.UsuarioActualizacion);
    }
}
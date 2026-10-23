using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

public class EstadoReservaConfiguration
    : IEntityTypeConfiguration<EstadoReserva>
{
    public void Configure(
        EntityTypeBuilder<EstadoReserva> builder)
    {
        builder.ToTable("EstadoReserva");

        builder.HasKey(e => e.IdEstadoReserva);

        builder.Property(e => e.IdEstadoReserva)
            .HasColumnName("IdEstadoReserva");

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
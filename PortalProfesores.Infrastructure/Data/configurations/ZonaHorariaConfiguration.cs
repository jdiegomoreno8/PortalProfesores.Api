using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

public class ZonaHorariaConfiguration
    : IEntityTypeConfiguration<ZonaHoraria>
{
    public void Configure(EntityTypeBuilder<ZonaHoraria> builder)
    {
        builder.ToTable("ZonaHoraria");

        builder.HasKey(z => z.IdZonaHoraria);

        builder.Property(z => z.IdZonaHoraria)
            .HasColumnName("IdZonaHoraria");

        builder.Property(z => z.Nombre)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(z => z.Codigo)
            .HasMaxLength(100)
            .IsRequired();

        // La tabla ZonaHoraria no contiene esta columna.
        builder.Ignore(z => z.Descripcion);

        builder.Property(z => z.FechaCreacion)
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(z => z.UsuarioCreacion);

        builder.Property(z => z.FechaActualizacion)
            .HasColumnType("datetime2");

        builder.Property(z => z.UsuarioActualizacion);

        builder.HasMany(z => z.Disponibilidades)
            .WithOne(d => d.ZonaHoraria)
            .HasForeignKey(d => d.IdZonaHoraria);

    }
}
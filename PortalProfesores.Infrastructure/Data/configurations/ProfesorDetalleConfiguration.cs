using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

public class ProfesorDetalleConfiguration
    : IEntityTypeConfiguration<ProfesorDetalle>
{
    public void Configure(EntityTypeBuilder<ProfesorDetalle> builder)
    {
        builder.ToTable("ProfesorDetalle");

        builder.HasKey(x => x.IdProfesor);

        builder.Property(x => x.IdProfesor)
               .HasColumnName("IdProfesor");

        builder.Property(x => x.IdPersona)
               .HasColumnName("IdPersona")
               .IsRequired();

        builder.Property(x => x.Presentacion)
               .HasColumnType("nvarchar(max)");

        builder.Property(x => x.Experiencia)
               .HasColumnType("nvarchar(max)");

        builder.Property(x => x.Biografia)
               .HasColumnType("nvarchar(max)");

        builder.Property(x => x.FechaIngreso)
               .IsRequired();

        builder.Property(x => x.FechaCreacion)
               .IsRequired();

        builder.Property(x => x.UsuarioCreacion);

        builder.Property(x => x.FechaActualizacion);

        builder.Property(x => x.UsuarioActualizacion);

        builder.HasOne(x => x.Persona)
               .WithOne()
               .HasForeignKey<ProfesorDetalle>(x => x.IdPersona)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
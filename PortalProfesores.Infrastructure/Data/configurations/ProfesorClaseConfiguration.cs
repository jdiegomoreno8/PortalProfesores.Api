
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

public class ProfesorClaseConfiguration
    : IEntityTypeConfiguration<ProfesorClase>
{
    public void Configure(EntityTypeBuilder<ProfesorClase> builder)
    {
        builder.ToTable("ProfesorClase");

        builder.HasKey(x => x.IdProfesorClase);

        builder.Property(x => x.IdProfesorClase)
            .HasColumnName("IdProfesorClase");

        builder.Property(x => x.IdProfesor)
            .IsRequired();

        builder.Property(x => x.IdClase)
            .IsRequired();

        builder.Property(x => x.FechaCreacion)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(x => x.UsuarioCreacion);

        builder.HasOne(x => x.Profesor)
            .WithMany()
            .HasForeignKey(x => x.IdProfesor)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Clase)
            .WithMany()
            .HasForeignKey(x => x.IdClase)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.IdProfesor, x.IdClase })
            .IsUnique()
            .HasDatabaseName("UQ_ProfesorClase_Profesor_Clase");
    }
}

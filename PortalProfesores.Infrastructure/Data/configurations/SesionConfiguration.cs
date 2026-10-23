using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración de Entity Framework para Sesion.
/// </summary>
public class SesionConfiguration
    : IEntityTypeConfiguration<Sesion>
{
    public void Configure(EntityTypeBuilder<Sesion> builder)
    {
        builder.ToTable("Sesion", table =>
        {
            table.HasCheckConstraint(
                "CK_Sesion_Fecha",
                "[FechaFinReal] IS NULL OR " +
                "[FechaInicioReal] IS NULL OR " +
                "[FechaFinReal] > [FechaInicioReal]");
        });

        builder.HasKey(s => s.IdSesion);

        builder.Property(s => s.IdSesion)
            .HasColumnName("IdSesion");

        builder.Property(s => s.IdReserva)
            .IsRequired();

        builder.Property(s => s.IdClase)
            .IsRequired();

        builder.Property(s => s.FechaInicioReal)
            .HasColumnType("datetime2");

        builder.Property(s => s.FechaFinReal)
            .HasColumnType("datetime2");

        builder.Property(s => s.ObservacionesProfesor)
            .HasColumnType("nvarchar(max)");

        builder.Property(s => s.FechaCreacion)
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        builder.Property(s => s.UsuarioCreacion);

        builder.Property(s => s.FechaActualizacion)
            .HasColumnType("datetime2");

        builder.Property(s => s.UsuarioActualizacion);

        builder.HasIndex(s => s.IdReserva)
            .IsUnique()
            .HasDatabaseName("UQ_Sesion_Reserva");

        builder.HasIndex(s => s.IdClase)
            .HasDatabaseName("IX_Sesion_IdClase");

        builder.HasOne(s => s.Clase)
            .WithMany(c => c.Sesiones)
            .HasForeignKey(s => s.IdClase)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Sesion_Clase");

        builder.HasOne(s => s.Reserva)
            .WithOne(r => r.Sesion)
            .HasForeignKey<Sesion>(s => s.IdReserva)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Sesion_Reserva");
    }
}
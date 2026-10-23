using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración de Entity Framework para Reserva.
/// </summary>
public class ReservaConfiguration
    : IEntityTypeConfiguration<Reserva>
{
    public void Configure(EntityTypeBuilder<Reserva> builder)
    {
        builder.ToTable("Reserva");

        builder.HasKey(r => r.IdReserva);

        builder.Property(r => r.IdReserva)
            .HasColumnName("IdReserva");

        builder.Property(r => r.IdDisponibilidad)
            .IsRequired();

        builder.Property(r => r.IdPersona)
            .IsRequired();

        builder.Property(r => r.IdEstadoReserva)
            .IsRequired();

        builder.Property(r => r.FechaReserva)
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(r => r.FechaExpiracion)
    .HasColumnType("datetime2")
    .IsRequired(false);

        builder.Property(r => r.Observacion)
            .HasMaxLength(1000);

        builder.Property(r => r.FechaCreacion)
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        builder.Property(r => r.UsuarioCreacion);

        builder.Property(r => r.FechaActualizacion)
            .HasColumnType("datetime2");

        builder.Property(r => r.UsuarioActualizacion);

      

        builder.HasIndex(r => r.FechaReserva)
            .HasDatabaseName("IX_Reserva_FechaReserva");

        builder.HasIndex(r => r.IdEstadoReserva)
            .HasDatabaseName("IX_Reserva_IdEstadoReserva");

        builder.HasIndex(r => r.IdPersona)
            .HasDatabaseName("IX_Reserva_IdPersona");

        builder.HasIndex(r => r.IdDisponibilidad)
    .HasDatabaseName("IX_Reserva_IdDisponibilidad");


        builder.HasOne(r => r.Disponibilidad)
            .WithMany()
            .HasForeignKey(r => r.IdDisponibilidad)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Reserva_Disponibilidad");



        builder.HasOne(r => r.Persona)
    .WithMany()
    .HasForeignKey(r => r.IdPersona)
    .OnDelete(DeleteBehavior.Restrict)
    .HasConstraintName("FK_Reserva_Persona");

        builder.HasOne(r => r.EstadoReserva)
     .WithMany(e => e.Reservas)
     .HasForeignKey(r => r.IdEstadoReserva)
     .OnDelete(DeleteBehavior.Restrict)
     .HasConstraintName("FK_Reserva_EstadoReserva");

        builder.HasOne(r => r.Sesion)
            .WithOne(s => s.Reserva)
            .HasForeignKey<Sesion>(s => s.IdReserva)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

public class DisponibilidadConfiguration
    : IEntityTypeConfiguration<Disponibilidad>
{
    public void Configure(EntityTypeBuilder<Disponibilidad> builder)
    {
        builder.ToTable("Disponibilidad", table =>
        {
            table.HasCheckConstraint(
                "CK_Disponibilidad_Horas",
                "[HoraInicio] < [HoraFin]");
        });

        builder.HasKey(d => d.IdDisponibilidad);

        builder.Property(d => d.IdDisponibilidad)
            .HasColumnName("IdDisponibilidad");

        builder.Property(d => d.IdProfesor)
            .IsRequired();

        builder.Property(d => d.IdEstadoDisponibilidad)
            .IsRequired();

        builder.Property(d => d.IdZonaHoraria)
            .IsRequired();

        builder.Property(d => d.Fecha)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(d => d.HoraInicio)
            .HasColumnType("time(7)")
            .IsRequired();

        builder.Property(d => d.HoraFin)
            .HasColumnType("time(7)")
            .IsRequired();

        builder.Property(d => d.Observacion)
            .HasMaxLength(600);

        builder.Property(d => d.FechaCreacion)
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        builder.Property(d => d.UsuarioCreacion);

        builder.Property(d => d.FechaActualizacion)
            .HasColumnType("datetime2");

        builder.Property(d => d.UsuarioActualizacion);

        builder.HasIndex(d => new
        {
            d.IdProfesor,
            d.Fecha,
            d.HoraInicio
        })
            .IsUnique()
            .HasDatabaseName("UQ_Disponibilidad_Profesor_Fecha_Hora");

        builder.HasIndex(d => d.Fecha)
            .HasDatabaseName("IX_Disponibilidad_Fecha");

        builder.HasIndex(d => d.IdEstadoDisponibilidad)
            .HasDatabaseName("IX_Disponibilidad_IdEstadoDisponibilidad");

        builder.HasIndex(d => d.IdProfesor)
            .HasDatabaseName("IX_Disponibilidad_IdProfesor");

        builder.HasIndex(d => d.IdZonaHoraria)
            .HasDatabaseName("IX_Disponibilidad_IdZonaHoraria");

        builder.HasOne(d => d.Profesor)
            .WithMany()
            .HasForeignKey(d => d.IdProfesor)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Disponibilidad_Profesor");

        builder.HasOne(d => d.EstadoDisponibilidad)
    .WithMany(e => e.Disponibilidades)
    .HasForeignKey(d => d.IdEstadoDisponibilidad)
    .OnDelete(DeleteBehavior.Restrict)
    .HasConstraintName("FK_Disponibilidad_Estado");

        builder.HasOne(d => d.ZonaHoraria)
            .WithMany()
            .HasForeignKey(d => d.IdZonaHoraria)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Disponibilidad_ZonaHoraria");



        builder.HasMany<Reserva>()
            .WithOne(r => r.Disponibilidad)
            .HasForeignKey(r => r.IdDisponibilidad)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Reserva_Disponibilidad");


    }
}
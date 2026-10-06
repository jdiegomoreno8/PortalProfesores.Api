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
        builder.ToTable("Sesion");

        // Clave primaria
        builder.HasKey(x => x.IdSesion);

        builder.Property(x => x.IdSesion)
               .HasColumnName("IdSesion");

        // Reserva
        builder.Property(x => x.IdReserva)
               .HasColumnName("IdReserva")
               .IsRequired();

        // Clase
        builder.Property(x => x.IdClase)
               .HasColumnName("IdClase")
               .IsRequired();

        builder.HasOne(x => x.Clase)
               .WithMany(x => x.Sesiones)
               .HasForeignKey(x => x.IdClase)
               .OnDelete(DeleteBehavior.Restrict);

        // Información de la sesión
        builder.Property(x => x.FechaInicioReal);

        builder.Property(x => x.FechaFinReal);

        builder.Property(x => x.ObservacionesProfesor)
               .HasColumnType("nvarchar(max)");

        // Auditoría
        builder.Property(x => x.FechaCreacion)
               .IsRequired();

        builder.Property(x => x.UsuarioCreacion);

        builder.Property(x => x.FechaActualizacion);

        builder.Property(x => x.UsuarioActualizacion);
    }
}
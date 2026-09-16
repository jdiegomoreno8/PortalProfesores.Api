using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración de la entidad Persona.
/// </summary>
public class PersonaConfiguration : IEntityTypeConfiguration<Persona>
{
    public void Configure(EntityTypeBuilder<Persona> builder)
    {
        builder.ToTable("Persona");

        builder.HasKey(x => x.IdPersona);

        builder.Property(x => x.IdPersona)
               .HasColumnName("IdPersona");

        builder.Property(x => x.IdTipoDocumento)
               .HasColumnName("IdTipoDocumento");

        builder.Property(x => x.IdCiudad)
               .HasColumnName("IdCiudad");

        builder.Property(x => x.NumeroDocumento)
               .HasMaxLength(120)
               .HasColumnName("NumeroDocumento");

        builder.Property(x => x.Nombres)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(x => x.Apellidos)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(x => x.CorreoElectronico)
               .HasMaxLength(300)
               .IsRequired();

        builder.Property(x => x.Telefono)
               .HasMaxLength(60);

        builder.Property(x => x.FechaNacimiento)
               .HasColumnType("date");

        builder.Property(x => x.FechaIngreso)
               .IsRequired();

        // Relación 1 a 1 entre Persona y Usuario
        builder.HasOne(x => x.Usuario)
               .WithOne(x => x.Persona)
               .HasForeignKey<Usuario>(x => x.IdPersona);

        // Relación entre Persona y TipoDocumento
        builder.HasOne(x => x.TipoDocumento)
               .WithMany(x => x.Personas)
               .HasForeignKey(x => x.IdTipoDocumento)
               .OnDelete(DeleteBehavior.Restrict);

        // Relación entre Persona y Ciudad
        builder.HasOne(x => x.Ciudad)
               .WithMany(x => x.Personas)
               .HasForeignKey(x => x.IdCiudad)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
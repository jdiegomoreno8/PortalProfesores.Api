using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración de la entidad Persona.
/// Solo se mapean las propiedades necesarias para el módulo de autenticación.
/// </summary>
public class PersonaConfiguration : IEntityTypeConfiguration<Persona>
{
    public void Configure(EntityTypeBuilder<Persona> builder)
    {
        builder.ToTable("Persona");

        builder.HasKey(x => x.IdPersona);

        builder.Property(x => x.IdPersona)
               .HasColumnName("IdPersona");

        builder.Property(x => x.Nombres)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(x => x.Apellidos)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(x => x.CorreoElectronico)
               .HasMaxLength(300)
               .IsRequired();

        // Relación 1 a 1 entre Persona y Usuario
        builder.HasOne(x => x.Usuario)
               .WithOne(x => x.Persona)
               .HasForeignKey<Usuario>(x => x.IdPersona);
    }
}
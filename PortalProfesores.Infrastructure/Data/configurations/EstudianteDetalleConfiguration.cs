using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración de Entity Framework para EstudianteDetalle.
///
/// Define la correspondencia entre la entidad de dominio
/// y la tabla EstudianteDetalle de SQL Server.
/// </summary>
public class EstudianteDetalleConfiguration
    : IEntityTypeConfiguration<EstudianteDetalle>
{
    public void Configure(EntityTypeBuilder<EstudianteDetalle> builder)
    {
        builder.ToTable("EstudianteDetalle");

        // ---------------------------------------------------------
        // CLAVE PRIMARIA
        // ---------------------------------------------------------
        builder.HasKey(x => x.IdEstudiante);

        builder.Property(x => x.IdEstudiante)
               .HasColumnName("IdEstudiante")
               .ValueGeneratedOnAdd();

        // ---------------------------------------------------------
        // RELACIÓN CON PERSONA
        // ---------------------------------------------------------

        // Todo estudiante debe estar asociado
        // a una Persona existente.
        builder.Property(x => x.IdPersona)
               .HasColumnName("IdPersona")
               .IsRequired();

        // Relación 1:1:
        // Persona -> EstudianteDetalle
        builder.HasOne(x => x.Persona)
               .WithOne()
               .HasForeignKey<EstudianteDetalle>(x => x.IdPersona)
               .OnDelete(DeleteBehavior.Restrict);

        // ---------------------------------------------------------
        // NIVEL
        // ---------------------------------------------------------

        // El nivel puede ser NULL porque un estudiante
        // puede registrarse antes de definir su nivel.
        builder.Property(x => x.IdNivel)
        .HasColumnName("IdNivel")
        .IsRequired();

        builder.HasOne(x => x.Nivel)
               .WithMany()
               .HasForeignKey(x => x.IdNivel)
               .OnDelete(DeleteBehavior.Restrict);

        // ---------------------------------------------------------
        // INFORMACIÓN ACADÉMICA
        // ---------------------------------------------------------

        builder.Property(x => x.ObjetivoAprendizaje)
               .HasColumnType("nvarchar(max)");

        builder.Property(x => x.Observaciones)
               .HasColumnType("nvarchar(max)");

        // ---------------------------------------------------------
        // Foto url
        // ---------------------------------------------------------
        builder.Property(x => x.FotoPerfilUrl)
            .HasColumnName("FotoPerfilUrl")
            .HasMaxLength(500)
            .IsRequired(false);


        // ---------------------------------------------------------
        // AUDITORÍA
        // ---------------------------------------------------------

        builder.Property(x => x.FechaIngreso)
               .IsRequired();

        builder.Property(x => x.FechaCreacion)
               .IsRequired();

        builder.Property(x => x.UsuarioCreacion);

        builder.Property(x => x.FechaActualizacion);

        builder.Property(x => x.UsuarioActualizacion);
    }
}
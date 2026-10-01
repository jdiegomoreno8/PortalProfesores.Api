using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración de Entity Framework para la entidad Nivel.
///
/// Define la correspondencia entre la entidad de dominio
/// y la tabla Nivel existente en SQL Server.
/// </summary>
public class NivelConfiguration
    : IEntityTypeConfiguration<Nivel>
{
    public void Configure(EntityTypeBuilder<Nivel> builder)
    {
        builder.ToTable("Nivel");

        // ---------------------------------------------------------
        // CLAVE PRIMARIA
        // ---------------------------------------------------------

        builder.HasKey(x => x.IdNivel);

        builder.Property(x => x.IdNivel)
               .HasColumnName("IdNivel")
               .ValueGeneratedOnAdd();

        // ---------------------------------------------------------
        // DATOS DEL CATÁLOGO
        // ---------------------------------------------------------

        builder.Property(x => x.Nombre)
               .HasColumnName("Nombre")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.Descripcion)
               .HasColumnName("Descripcion")
               .HasMaxLength(500);

        // ---------------------------------------------------------
        // AUDITORÍA
        // ---------------------------------------------------------

        builder.Property(x => x.FechaCreacion)
               .HasColumnName("FechaCreacion")
               .IsRequired();

        builder.Property(x => x.UsuarioCreacion)
               .HasColumnName("UsuarioCreacion");

        builder.Property(x => x.FechaActualizacion)
               .HasColumnName("FechaActualizacion");

        builder.Property(x => x.UsuarioActualizacion)
               .HasColumnName("UsuarioActualizacion");

        // ---------------------------------------------------------
        // REGLA DEL CATÁLOGO
        // ---------------------------------------------------------

        // El nombre del nivel es único en la base de datos.
        // Por ejemplo, no pueden existir dos registros llamados "A1".
        builder.HasIndex(x => x.Nombre)
               .IsUnique()
               .HasDatabaseName("UQ_Nivel_Nombre");
    }
}
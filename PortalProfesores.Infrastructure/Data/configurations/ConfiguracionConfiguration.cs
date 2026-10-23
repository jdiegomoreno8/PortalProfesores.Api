
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

public class ConfiguracionConfiguration
    : IEntityTypeConfiguration<Configuracion>
{
    public void Configure(EntityTypeBuilder<Configuracion> builder)
    {
        builder.ToTable("Configuracion", "dbo");

        builder.HasKey(x => x.IdConfiguracion);

        builder.Property(x => x.IdConfiguracion)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Clave)
            .IsRequired();

        builder.Property(x => x.Valor)
            .IsRequired();

        builder.Property(x => x.Descripcion);

        builder.Property(x => x.TipoDato)
            .IsRequired();

        builder.Property(x => x.FechaCreacion)
            .IsRequired();

        builder.Property(x => x.UsuarioCreacion);

        builder.Property(x => x.FechaActualizacion);

        builder.Property(x => x.UsuarioActualizacion);

        builder.HasIndex(x => x.Clave)
            .IsUnique();
    }
}

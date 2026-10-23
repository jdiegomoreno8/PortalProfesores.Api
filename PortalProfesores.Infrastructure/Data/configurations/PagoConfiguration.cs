using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalProfesores.Domain.Entities;

namespace PortalProfesores.Infrastructure.Data.Configurations;

public class PagoConfiguration : IEntityTypeConfiguration<Pago>
{
    public void Configure(EntityTypeBuilder<Pago> builder)
    {
        builder.ToTable("Pago");

        // Clave primaria
        builder.HasKey(p => p.IdPago);

        builder.Property(p => p.IdPago)
            .HasColumnName("IdPago");

        // Campos principales
        builder.Property(p => p.IdReserva).IsRequired();
        builder.Property(p => p.IdEstadoPago).IsRequired();
        builder.Property(p => p.IdMetodoPago).IsRequired();
        builder.Property(p => p.IdMoneda).IsRequired();

        builder.Property(p => p.ValorOriginal)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.TasaCambio)
            .HasColumnType("decimal(18,8)");

        builder.Property(p => p.ValorConvertido)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.FechaInicioPago)
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(p => p.FechaPago)
            .HasColumnType("datetime2");

        builder.Property(p => p.IdTransaccionExterna)
            .HasMaxLength(300);

        builder.Property(p => p.Observacion)
            .HasMaxLength(1000);

        builder.Property(p => p.FechaCreacion)
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(p => p.UsuarioCreacion);

        builder.Property(p => p.FechaActualizacion)
            .HasColumnType("datetime2");

        builder.Property(p => p.UsuarioActualizacion);

        // Índices existentes en la base de datos
        builder.HasIndex(p => p.IdEstadoPago)
            .HasDatabaseName("IX_Pago_IdEstadoPago");

        builder.HasIndex(p => p.IdMetodoPago)
            .HasDatabaseName("IX_Pago_IdMetodoPago");

        builder.HasIndex(p => p.IdMoneda)
            .HasDatabaseName("IX_Pago_IdMoneda");

        builder.HasIndex(p => p.IdReserva)
            .HasDatabaseName("IX_Pago_IdReserva");

        builder.HasIndex(p => p.IdTransaccionExterna)
            .HasDatabaseName("IX_Pago_IdTransaccionExterna");



        // Relaciones con las entidades existentes
        builder.HasOne(p => p.Reserva)
            .WithMany()
            .HasForeignKey(p => p.IdReserva)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Pago_Reserva");

        builder.HasOne(p => p.EstadoPago)
            .WithMany(e => e.Pagos)
            .HasForeignKey(p => p.IdEstadoPago)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Pago_Estado");

        builder.HasOne(p => p.MetodoPago)
            .WithMany(m => m.Pagos)
            .HasForeignKey(p => p.IdMetodoPago)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Pago_Metodo");

        builder.HasOne(p => p.Moneda)
            .WithMany(m => m.Pagos)
            .HasForeignKey(p => p.IdMoneda)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Pago_Moneda");
    }
}
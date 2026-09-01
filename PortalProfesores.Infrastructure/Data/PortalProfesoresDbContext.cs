using Microsoft.EntityFrameworkCore;

namespace PortalProfesores.Infrastructure.Data;

public class PortalProfesoresDbContext : DbContext
{
    public PortalProfesoresDbContext(DbContextOptions<PortalProfesoresDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
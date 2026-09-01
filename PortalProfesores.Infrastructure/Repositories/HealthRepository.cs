using Microsoft.EntityFrameworkCore;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Infrastructure.Data;

namespace PortalProfesores.Infrastructure.Repositories;

public class HealthRepository : IHealthRepository
{
    private readonly PortalProfesoresDbContext _context;

    public HealthRepository(PortalProfesoresDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CanConnectAsync()
    {
        return await _context.Database.CanConnectAsync();
    }
}
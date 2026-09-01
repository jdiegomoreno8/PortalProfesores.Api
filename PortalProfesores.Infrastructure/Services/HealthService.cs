using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Domain.Interfaces.Services;

namespace PortalProfesores.Infrastructure.Services;

public class HealthService : IHealthService
{
    private readonly IHealthRepository _repository;

    public HealthService(IHealthRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> CanConnectAsync()
    {
        return await _repository.CanConnectAsync();
    }
}
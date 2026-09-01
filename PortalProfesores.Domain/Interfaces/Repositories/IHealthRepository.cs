namespace PortalProfesores.Domain.Interfaces.Repositories;

public interface IHealthRepository
{
    Task<bool> CanConnectAsync();
}
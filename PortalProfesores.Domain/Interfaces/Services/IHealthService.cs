namespace PortalProfesores.Domain.Interfaces.Services;

public interface IHealthService
{
    Task<bool> CanConnectAsync();
}
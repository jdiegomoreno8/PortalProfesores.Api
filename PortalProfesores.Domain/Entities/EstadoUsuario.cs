namespace PortalProfesores.Domain.Entities;

public class EstadoUsuario
{
    public long IdEstadoUsuario { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    // Navegación
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}